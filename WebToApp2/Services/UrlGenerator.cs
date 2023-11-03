namespace WebToApp2.Services
{
    public class UrlGenerator
    {
        private static string? _baseUrl;

        public UrlGenerator(IHttpContextAccessor httpContextAccessor)
        {
            _baseUrl =
                $"https://{httpContextAccessor.HttpContext?.Request.Host.ToUriComponent()}";
        }

        public string GenerateGetFileUrl(string encodedString)
        {
            if (string.IsNullOrEmpty(encodedString))
                return string.Empty;

            return _baseUrl + "/" + ApiRoutes.Auth.GetFile + "/?tsquery=" + encodedString;
        }

        public string GenerateCallbackUrl()
        {
            return _baseUrl + "/" + ApiRoutes.Auth.Callback;
        }
        
        public string GenerateDataUrl(string token)
        {
            return _baseUrl + "/" + ApiRoutes.Auth.DataUri + "?token=" + token;
        }
    }
}
