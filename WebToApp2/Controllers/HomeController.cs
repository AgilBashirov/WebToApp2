using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using WebToApp2.Models;
using WebToApp2.Services;

namespace WebToApp2.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IAuthService _authService;

        public HomeController(IAuthService authService) => _authService = authService;

        //[HttpGet(ApiRoutes.Auth.GenerateQr)]
        //public async Task<IActionResult> GenerateQrCodeForLogin() =>
        //Ok(await _authService.GenerateQrCodeAsync());

        [HttpGet(ApiRoutes.Auth.GenerateQr)]
        public async Task<FileResult> GenerateQrForLogin() =>
        new FileContentResult(await _authService.GenerateQrForLogin(), MediaTypeNames.Image.Jpeg);

        [HttpGet(ApiRoutes.Auth.DataUri)]
        public async Task<IActionResult> GetData(string token) =>
            Ok(await _authService.GetDataAsync(token));

        [HttpGet(ApiRoutes.Auth.GetFile)]
        public async Task<IActionResult> GetFile([FromQuery] string tsquery) =>
        Ok(await _authService.GetFileAsync(tsquery));
        
        [HttpPost(ApiRoutes.Auth.Callback)]
        public async Task<IActionResult> Callback([FromBody] CallbackPostRequest request) =>
            Ok(await _authService.ApproveCallBackAsync(request));
    }
}
