using System.Threading.Tasks;
using SignalRApp.Business.DTOs.Request;
using SignalRApp.Business.Utilities.Results;
using SignalRApp.Business.Utilities.Security.Jwt;

namespace SignalRApp.Business.Services.Abstract
{
    public interface IAuthService
    {
        Task<DataResult<Token>> Register(RegisterDto registerDto);
        Task<DataResult<Token>> Login(LoginDto loginDto);
        Task<DataResult<Token>> RefreshToken(string refreshToken);
    }
}