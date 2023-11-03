namespace WebToApp2.Models
{
    public class GetFileResponse
    {
    }

    public class GetFileSuccessResponse : GetFileResponse
    {
        public string? Type { get; init; }

        public List<DataObject> DataObjects { get; set; }
    }
    public class DataObject
    {
        public string Name { get; set; } = null!;
        public string Data { get; set; } = null!;
    }

    public class GetFileErrorResponse : GetFileResponse
    {
        public string ErrorMessage { get; init; }
    }
}
