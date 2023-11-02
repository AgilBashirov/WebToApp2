using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebToApp2.Services;

namespace WebToApp2.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IAuthService _authService;

        public HomeController(IAuthService authService) => _authService = authService;

        [HttpGet(ApiRoutes.Auth.GenerateQr)]
        public async Task<IActionResult> GenerateQrCodeForLogin() =>
        Ok(await _authService.GenerateQrCodeAsync());
    }
}
