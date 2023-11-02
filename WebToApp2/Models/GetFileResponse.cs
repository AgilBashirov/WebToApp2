namespace WebToApp2.Models
{
    public class GetFileResponse
    {
    }

    public class GetFileSuccessResponse : GetFileResponse
    {
        public string FileName { get; init; }

        public string Data { get; set; }
    }

    public class GetFileErrorResponse : GetFileResponse
    {
        public string ErrorMessage { get; init; }
    }
}
