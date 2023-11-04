using System.Reflection.Emit;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Shared.Exceptions;
using Shared.Helpers;
using WebToApp2.Entities;
using WebToApp2.Enum;
using WebToApp2.Models;
using WebToApp2.Models.Sima;

namespace WebToApp2.Services
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
            var encodedContract = EncodeContract(contract);
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
                .Include(x=>x.OperationFiles)
                .FirstOrDefaultAsync(x =>
                x.QrOperationId == container.SignableContainer.OperationInfo.OperationId!);
            
            
            
            var dataObjects = new List<DataObject>();
            dataObjects.Add(new DataObject
            {
                Name = "HUHU",
                Data = _urlGenerator.GenerateDataUrl("6524baefec9ef5c5fb858850546eb74e")
            });
            dataObjects.Add(new DataObject
            {
                Name = "HUHU",
                Data = _urlGenerator.GenerateDataUrl("6524baefec9ef5c5fb858850546eb74e")
            });

            var response = new GetFileSuccessResponse()
            {
                DataObjects = dataObjects,
                Type = "Sign"
            };

            return response;
        }

        public Task<CallbackPostResponse> ApproveCallBackAsync(CallbackPostRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetDataAsync(string token)
        {
            var base64Data = await token.GetFileBase64Async() ?? throw new NotFoundException("File not found");

            return base64Data;
        }

        private async Task AddOperations(IEnumerable<int> documentIds, string operationId, byte[] signature,
            GetFileResponseTypeEnum responseTypeEnum)
        {
            var operation = new Operation()
            {
                QrSignature = signature.ToString()!,
                QrOperationId = operationId,
                GetFileResponseType = responseTypeEnum,
            };
            foreach (var documentId in documentIds)
            {
                operation.OperationFiles.Add(new OperationFile()
                {
                    FileId = documentId,
                    OperationId = operation.Id
                });
            }
            await _context.Operations.AddAsync(operation);
            await _context.SaveChangesAsync();
        }
    }
}