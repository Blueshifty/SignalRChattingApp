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
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            return ResponseCreator.CreateDataResponse(await _userService.Login(loginDto));
        }

        [HttpPost]
        public async Task<ActionResult> Register([FromBody] RegisterDto registerDto)
        {
            return ResponseCreator.CreateDataResponse(await _userService.Register(registerDto));
        }

        [Authorize]
        [HttpGet]
        public string TokenTest()
        {
            return "Your Token Is Valid";
        }
    }
}