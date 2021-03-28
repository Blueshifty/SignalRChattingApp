using System.Linq;
using FluentValidation;

namespace SignalRApp.Business.Utilities.Validators
{
    public static class ValidatorHelper<T> where T : class
    {
        public static bool Validate(AbstractValidator<T> validator, T dto, out string errors)
        {
            var result = validator.Validate(dto);

            errors = string.Concat(result.Errors.Select(x => x.ErrorMessage.Replace("'", "") + " "));

            return !string.IsNullOrEmpty(errors);
        }
    }
}