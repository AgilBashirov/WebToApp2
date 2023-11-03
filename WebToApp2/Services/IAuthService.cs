using WebToApp2.Models;

namespace WebToApp2.Services
{
    public interface IAuthService
    {
        public Task<byte[]> GenerateQrForLogin();
        //public Task<GenerateQrResponse> GenerateQrCodeAsync(string? operationId = null);
        public Task<GetFileResponse> GetFileAsync(string tsQuery);
        public Task<CallbackPostResponse> ApproveCallBackAsync(CallbackPostRequest request);
        public Task<string> GetDataAsync(string token);
    }
}
