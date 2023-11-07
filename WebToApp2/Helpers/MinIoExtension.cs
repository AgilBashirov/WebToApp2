using System.Net;
using System.Text.Json.Serialization;
using RestSharp;
using Shared.Exceptions;
using WebToApp2.Helpers;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Shared.Helpers;

public static class MinIoExtension
{
    public static async Task<string> GetFileUrlAsync(this string fileId)
    {
        GetResponseModel? result = null;

        try
        {
            using var client =
                new RestClient(
                    ConfigurationAccessor.AppConfiguration!["min.io:server"]); //automatically disposes after use
            var request = new RestRequest($"download/getURL");
            request.AddParameter("id", fileId, ParameterType.QueryString);
            request.AddHeader("appname", ConfigurationAccessor.AppConfiguration["min.io:appname"]);
            var saveResponse = await client.ExecuteAsync(request);

            if (saveResponse.IsSuccessful)
                result = JsonSerializer.Deserialize<GetResponseModel>(saveResponse.Content!);

            return result?.Data?.Url!;
        }
        catch (Exception)
        {
            throw new NotFoundException("File not found");
        }
    }

    public static async Task<string> GetFileBase64Async(this string fileId)
    {
        GetBase64ResponseModel? result = null;

        try
        {
            var a = ConfigurationAccessor.AppConfiguration!["min.io:server"];
            using var client =
                new RestClient(
                    ConfigurationAccessor.AppConfiguration!["min.io:server"]); //automatically disposes after use
            var request = new RestRequest($"download/base64");
            request.AddParameter("id", fileId, ParameterType.QueryString);
            request.AddHeader("appname", ConfigurationAccessor.AppConfiguration["min.io:appname"]);
            var saveResponse = await client.ExecuteAsync(request);

            if (saveResponse.IsSuccessful)
                result = JsonSerializer.Deserialize<GetBase64ResponseModel>(saveResponse.Content!);
            else throw new NotFoundException("File not found");

            return result?.Data?.Base64!;
        }
        catch (Exception)
        {
            throw new NotFoundException("File not found");
        }
    }

    public static async Task<string> SaveFileAsByteAsync(this IFormFile formFile)
    {
        try
        {
            var conf = ConfigurationAccessor.AppConfiguration!["min.io:server"];
            using var client = new RestClient(conf);
            var request = new RestRequest("upload/formdata", Method.Post);
            var fileBytes = await formFile.ToByte()!;
            request.AddFile("uploaded_file", fileBytes, formFile.FileName);

            request.AddHeader(HttpRequestHeader.ContentType.ToString(), " multipart/form-data");

            request.AddHeader("appname", ConfigurationAccessor.AppConfiguration["min.io:appname"]);

            var response = await client.ExecuteAsync(request);

            var resp = JsonSerializer.Deserialize<SaveResponseModel>(response.Content!);

            return resp?.Data!.File!.Id!;
        }
        catch (Exception ex)
        {
            throw new FileServerException(ex.Message, ex);
        }
    }

    public static async Task<string> SaveFileAsBaseAsync(this byte[] byteArray)
    {
        try
        {
            using var client = new RestClient(ConfigurationAccessor.AppConfiguration!["min.io:server"]);
            var request = new RestRequest("upload/base64", Method.Post);

            request.AddJsonBody(new { file = byteArray });
            request.AddHeader(HttpRequestHeader.ContentType.ToString(), "application/json");
            request.AddHeader("appname", ConfigurationAccessor.AppConfiguration["min.io:appname"]);

            var response = await client.ExecuteAsync(request);

            var resp = JsonSerializer.Deserialize<SaveBase64ResponseModel>(response.Content!);

            return resp?.Data!.File!.Id!;
        }
        catch (Exception ex)
        {
            throw new FileServerException(ex.Message, ex);
        }
    }

    private static Task<byte[]>? ToByte(this IFormFile formFile)
    {
        if (formFile.Length <= 0) return null;
        using var ms = new MemoryStream();
        formFile.CopyTo(ms);
        return Task.FromResult(ms.ToArray());
    }
}

public class SaveResponseModel
{
    [JsonPropertyName("data")] public SaveResponseData? Data { get; set; }

    [JsonPropertyName("error")] private bool Error { get; init; }

    // [JsonPropertyName("exception")] private string? Exception { get; init; }
}

public class SaveBase64ResponseModel
{
    [JsonPropertyName("data")] public SaveBase64ResponseData? Data { get; set; }

    [JsonPropertyName("error")] private bool Error { get; init; }

    /*[JsonPropertyName("exception")] private string? Exception { get; init; }*/
}

internal class GetResponseModel
{
    [JsonPropertyName("data")] public GetResponseData? Data { get; set; }

    [JsonPropertyName("error")] private bool Error { get; set; }

    // [JsonPropertyName("exception")] private string? Exception { get; set; }
}

internal class GetBase64ResponseModel
{
    [JsonPropertyName("data")] public GetBase64ResponseData? Data { get; set; }

    [JsonPropertyName("error")] private bool Error { get; set; }

    // [JsonPropertyName("exception")] private string? Exception { get; set; }
}

public class GetResponseData
{
    [JsonPropertyName("url")] public string? Url { get; set; }

    [JsonPropertyName("size")] private string? Size { get; init; }

    [JsonPropertyName("name")] private string? Name { get; init; }

    [JsonPropertyName("mimeType")] private string? MimeType { get; init; }
}

public class GetBase64ResponseData
{
    [JsonPropertyName("base64")] public string? Base64 { get; set; }

    [JsonPropertyName("size")] private string? Size { get; init; }

    [JsonPropertyName("name")] private string? Name { get; init; }

    [JsonPropertyName("mimeType")] private string? MimeType { get; init; }
}

public class SaveResponseData
{
    [JsonPropertyName("file")] public FileInfo? File { get; set; }
}

public class SaveBase64ResponseData
{
    [JsonPropertyName("fileData")] public FileInfo? File { get; set; }
}

public class FileInfo
{
    [JsonPropertyName("id")] public string? Id { get; set; }

    [JsonPropertyName("size")] public long Size { get; set; }

    [JsonPropertyName("mimeType")] public string? MimeType { get; set; }
}