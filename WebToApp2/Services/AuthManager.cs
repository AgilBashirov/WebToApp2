using System.Reflection.Emit;
using WebToApp2.Enum;
using WebToApp2.Models;

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

        public Task<GenerateQrResponse> GenerateQrCodeAsync(string? operationId = null)
        {
            var operationType = operationId == null ? OperationTypeEnum.Auth : OperationTypeEnum.Sign;
            operationId = operationId ?? Guid.NewGuid().ToString();
            var contract = CreateContract(operationId, operationType);
            contract.Header.Signature = CreateSignature(contract.SignableContainer);
            var encodedContract = EncodeContract(contract);
            var qrUrl = _urlGenerator.GenerateGetFileUrl(encodedContract);
            var qrCodeBase64 = _generatorService.GenerateQr(qrUrl).Result;
            var result = new GenerateQrResponse { QrCode = qrCodeBase64, OperationId = operationId };

            return Task.FromResult(result);
        }

        public Task<GetFileResponse> GetFileAsync(string tsQuery)
        {
            throw new NotImplementedException();
        }

        public Task<CallbackPostResponse> ApproveCallBackAsync(CallbackPostRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
