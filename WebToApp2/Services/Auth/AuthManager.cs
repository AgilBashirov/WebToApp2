using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Shared.Exceptions;
using Shared.Helpers;
using WebToApp2.Entities;
using WebToApp2.Enum;
using WebToApp2.Helpers;
using WebToApp2.Models;
using WebToApp2.Models.Sima;

namespace WebToApp2.Services.Auth
{
    public partial class AuthManager : IAuthService
    {
        private readonly UrlGenerator _urlGenerator;
        private readonly IGeneratorService _generatorService;
        private readonly ApplicationDbContext _context;

        public AuthManager(IGeneratorService generatorService, UrlGenerator urlGenerator, ApplicationDbContext context)
        {
            _generatorService = generatorService;
            _urlGenerator = urlGenerator;
            _context = context;
        }

        //public Task<GenerateQrResponse> GenerateQrCodeAsync(string? operationId = null)
        //{
        //    var operationType = operationId == null ? OperationTypeEnum.Auth : OperationTypeEnum.Sign;
        //    operationId = operationId ?? Guid.NewGuid().ToString();
        //    var contract = CreateContract(operationId, operationType);
        //    contract.Header.Signature = CreateSignature(contract.SignableContainer);
        //    var encodedContract = EncodeContract(contract);
        //    var qrUrl = _urlGenerator.GenerateGetFileUrl(encodedContract);
        //    var qrCodeBase64 = _generatorService.GenerateQr(qrUrl).Result;
        //    var result = new GenerateQrResponse { QrCode = qrCodeBase64, OperationId = operationId };

        //    return Task.FromResult(result);
        //}

        public async Task<byte[]> GenerateQrForLogin(List<int> documentIds, GetFileResponseTypeEnum responseTypeEnum)
        {
            var operationId = Guid.NewGuid().ToString();
            var contract = CreateContract(operationId);
            contract.Header.Signature = CreateSignature(contract.SignableContainer);
            var encodedContract = AuthManager.EncodeContract(contract);
            var qrUrl = _urlGenerator.GenerateGetFileUrl(encodedContract);
            // Associating operationId with documents to be signed
            await AddOperations(documentIds, operationId, contract.Header.Signature, responseTypeEnum);
            return await _generatorService.GenerateQr(qrUrl);
        }

        public async Task<GetFileResponse> GetFileAsync(string tsquery)
        {
            if (JsonSerializer.Deserialize<Contract>(
                    Encoding.UTF8.GetString(Convert.FromBase64String(tsquery))) is not { } container)
                throw new NullReferenceException(nameof(tsquery));

            var operation = await _context.Operations
                                .Include(x => x.OperationFiles)
                                .ThenInclude(x => x.AppFile)
                                .FirstOrDefaultAsync(x =>
                                    x.QrOperationId == container.SignableContainer.OperationInfo.OperationId!) ??
                            throw new NotFoundException("operation not found");


            var dataObjects = new List<DataObject>();
            foreach (var document in operation.OperationFiles.ToList())
            {
                dataObjects.Add(new DataObject
                {
                    Name = document.AppFile.Name,
                    Data = operation.GetFileResponseType is GetFileResponseTypeEnum.Url
                        ? _urlGenerator.GenerateDataUrl(document.AppFile.Token)
                        : await document.AppFile.Token.GetFileBase64Async()
                });
            }

            return new GetFileSuccessResponse
            {
                DataObjects = dataObjects,
                Type = operation.GetFileResponseType is GetFileResponseTypeEnum.Raw ? "Raw" : "Url"
            };
        }

        public async Task<CallbackPostResponse> ApproveCallBackAsync(CallbackPostRequest request)
        {
            var operation = await _context.Operations
                                .Include(x => x.OperationFiles)
                                .ThenInclude(x => x.AppFile)
                                .FirstOrDefaultAsync(x => x.QrOperationId == request.OperationId) ??
                            throw new NotFoundException("operation not found");
            
            var secretKeyHash = Encoding.UTF8.GetBytes(ConfigurationAccessor.AppConfiguration!["SimaContainer:Header:SecretKey"]!);
            var sha256Hash = CryptHelper.ComputeSha256HashAsByte((secretKeyHash + operation.QrSignature));
            var kId = Convert.ToBase64String(sha256Hash);
            
            if (operation.OperationFiles.All(x => x.AppFile.Name.Trim() != request.DataId.Trim()))
                throw new NotFoundException("File not found");
            
            operation.OperationFiles
                .FirstOrDefault(x => x.AppFile.Name.Trim() == request.DataId.Trim())!.IsSigned = true;

            _context.Operations.Update(operation);
            await _context.SaveChangesAsync();
            
            return new CallbackPostResponse { Status = "success" };
        }

        public async Task<string> GetDataAsync(string token)
        {
            var base64Data = await token.GetFileBase64Async() ?? throw new NotFoundException("File not found");

            return base64Data;
        }

        private async Task AddOperations(List<int> documentIds, string operationId, byte[] signature,
            GetFileResponseTypeEnum responseTypeEnum)
        {
            bool isExist = documentIds.All(documentId => _context.Files.Select(x => x.Id).Contains(documentId));
            if (documentIds is null || documentIds.Count == 0 || !isExist)
                throw new Exception("File not fount");
            var operation = new Operation()
            {
                QrSignature = JsonSerializer.Serialize(signature).Replace("\"", ""),
                QrOperationId = operationId,
                GetFileResponseType = responseTypeEnum
            };
            documentIds.ToList().ForEach(documentId =>
                operation.OperationFiles.Add(new OperationFile() { AppFileId = documentId }));

            await _context.Operations.AddAsync(operation);
            await _context.SaveChangesAsync();
        }
    }
}