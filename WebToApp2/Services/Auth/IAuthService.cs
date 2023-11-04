using WebToApp2.Enum;
using WebToApp2.Models;

namespace WebToApp2.Services
{
    public interface IAuthService
    {
        public Task<byte[]> GenerateQrForLogin(List<int> documentIds, GetFileResponseTypeEnum responseTypeEnum);
        //public Task<GenerateQrResponse> GenerateQrCodeAsync(string? operationId = null);
        public Task<GetFileResponse> GetFileAsync(string tsQuery);
        public Task<CallbackPostResponse> ApproveCallBackAsync(CallbackPostRequest request);
        public Task<string> GetDataAsync(string token);
    }
}
