using WebToApp2.DTOs;

namespace WebToApp2.Services.File
{
    public interface IFileService
    {
        Task CreateFileAsync(IFormFile file);
        Task<List<FileDto>> GetAllFilesAsync();
    }
}
