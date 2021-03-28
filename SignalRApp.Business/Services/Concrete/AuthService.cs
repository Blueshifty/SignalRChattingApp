using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SignalRApp.Business.DTOs.Request;
using SignalRApp.Business.Services.Abstract;
using SignalRApp.Business.Utilities.Mapper;
using SignalRApp.Business.Utilities.Results;
using SignalRApp.Business.Utilities.Security.Hashing;
using SignalRApp.Business.Utilities.Security.Jwt;
using SignalRApp.Business.Utilities.Validators;
using SignalRApp.Data.EntityFramework;
using SignalRApp.Data.Models;

namespace SignalRApp.Business.Services.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly SignalRAppDbContext _context;

        private readonly IMapper _mapper;

        private readonly ITokenHandler _tokenHandler;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(SignalRAppDbContext context, IMapper mapper, ITokenHandler tokenHandler,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;

            _mapper = mapper;

            _tokenHandler = tokenHandler;

            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<DataResult<Token>> Register(RegisterDto registerDto)
        {
            if (ValidatorHelper<RegisterDto>.Validate(new UserValidator(), registerDto, out var errors))
                return new DataResult<Token>(errors, false);

            if (_context.Users.FirstOrDefault(u => u.UserName == registerDto.UserName) != null)
                return new DataResult<Token>("Bu Kullanıcı Adı Zaten Alınmış", false);

            var user = _mapper.Map<RegisterDto, User>(registerDto);

            HashingHelper.CreatePasswordHash(registerDto.Password, out var passwordHash, out var passwordSalt);

            user.PasswordHash = passwordHash;

            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();

            var token = _tokenHandler.CreateAccessToken(user);

            user.RefreshToken = token.RefreshToken;

            await _context.SaveChangesAsync();

            return new DataResult<Token>(token);
        }

        public async Task<DataResult<Token>> Login(LoginDto loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username);

            if (user == null ||
                !HashingHelper.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt))
                return new DataResult<Token>("Kullanıcı Adı veya Şifre Yanlış", false);

            var token = _tokenHandler.CreateAccessToken(user);

            user.RefreshToken = token.RefreshToken;

            await _context.SaveChangesAsync();

            return new DataResult<Token>(token);
        }

        public async Task<DataResult<Token>> RefreshToken(string refreshToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

            if (user == null) return new DataResult<Token>("Token is not valid", false);

            var jwtToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");

            var jwtResolver = new JwtResolver(jwtToken);

            var userId = jwtResolver.GetClaim(ClaimTypes.NameIdentifier);
            // More Secure
            if (userId != user.Id.ToString()) return new DataResult<Token>("Your Jwt Token is not valid", false);

            var token = _tokenHandler.CreateAccessToken(user);

            user.RefreshToken = token.RefreshToken;

            await _context.SaveChangesAsync();

            return new DataResult<Token>(token);
        }
    }
}