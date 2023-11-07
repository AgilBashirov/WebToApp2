using Microsoft.EntityFrameworkCore;
using Shared.Helpers;
using WebToApp2.DTOs;

namespace WebToApp2.Services.File
{
    public class FileManager : IFileService
    {
        private readonly ApplicationDbContext _context;
        public FileManager(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task CreateFileAsync(IFormFile formFile)
        {
            var token = await formFile.SaveFileAsByteAsync();
            var fileName = Path.GetFileName(formFile.FileName);
            var fileExtension = Path.GetExtension(formFile.FileName);

            var file = await _context.Files.AddAsync(new Entities.AppFile
            {
                Name = fileName,
                Extension = fileExtension,
                Token = token
            });
            await _context.SaveChangesAsync();
        }

        public async Task<List<FileDto>> GetAllFilesAsync()
        {
            List<FileDto> response = new List<FileDto>();
            var files = await _context.Files.ToListAsync();

            foreach (var file in files)
            {
                var fileUrl = await file.Token.GetFileUrlAsync();
                response.Add(new FileDto { Id = file.Id, Extension = file.Extension, Name = file.Name, FileUrl = fileUrl, Token = file.Token});
            }

            return response;

        }
    }
}
