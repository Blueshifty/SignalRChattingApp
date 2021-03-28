using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalRApp.Business.DTOs.Request;
using SignalRApp.Business.Services.Abstract;
using SignalRApp.Utilities;

namespace SignalRApp.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            return ResponseCreator.CreateDataResponse(await _authService.Login(loginDto));
        }

        [HttpPost]
        public async Task<ActionResult> Register([FromBody] RegisterDto registerDto)
        {
            return ResponseCreator.CreateDataResponse(await _authService.Register(registerDto));
        }

        [HttpPost]
        public async Task<ActionResult> RefreshToken(string token)
        {
            return ResponseCreator.CreateDataResponse(await _authService.RefreshToken(token));
        }

        [Authorize]
        [HttpGet]
        public string TokenTest()
        {
            return "Your Token Is Valid";
        }
    }
}