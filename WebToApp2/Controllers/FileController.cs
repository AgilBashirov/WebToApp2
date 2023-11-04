using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebToApp2.Services.File;

namespace WebToApp2.Controllers
{
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        public FileController(IFileService fileService) =>
         _fileService = fileService;

        [HttpGet(ApiRoutes.File.GetAll)]
        public async Task<IActionResult> GetAll() =>
        Ok(await _fileService.GetAllFilesAsync());

        [HttpPost(ApiRoutes.File.Create)]
        public async Task Create(IFormFile file) =>
        await _fileService.CreateFileAsync(file);
    }
}
