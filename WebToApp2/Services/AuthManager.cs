using System.Reflection.Emit;
using Shared.Exceptions;
using Shared.Helpers;
using WebToApp2.Enum;
using WebToApp2.Models;
using WebToApp2.Models.Sima;

namespace WebToApp2.Services
{
    public partial class AuthManager : IAuthService
    {
        private readonly UrlGenerator _urlGenerator;
        private readonly IGeneratorService _generatorService;

        public AuthManager(IGeneratorService generatorService, UrlGenerator urlGenerator)
        {
            _generatorService = generatorService;
            _urlGenerator = urlGenerator;
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
        public Task<byte[]> GenerateQrForLogin()
        {
            var operationId = Guid.NewGuid().ToString();
            var contract = CreateContract(operationId);
            contract.Header.Signature = CreateSignature(contract.SignableContainer);
            var encodedContract = EncodeContract(contract);
            var qrUrl = _urlGenerator.GenerateGetFileUrl(encodedContract);
            return _generatorService.GenerateQr(qrUrl);
        }

        public async Task<GetFileResponse> GetFileAsync(string tsQuery)
        {
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
    }
}
