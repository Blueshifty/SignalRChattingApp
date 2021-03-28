using FluentValidation;
using SignalRApp.Business.DTOs.Request;

namespace SignalRApp.Business.Utilities.Validators
{
    public class UserValidator : AbstractValidator<RegisterDto>
    {
        public UserValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithName("Kullanıcı Adı");
            RuleFor(x => x.Password).NotEmpty().WithName("Şifre");
        }
    }
}