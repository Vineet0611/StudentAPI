using FluentValidation;

namespace StudentApi.Features.AuthFeatures.Login
{
    public class LoginValidator : AbstractValidator<LoginRequestModel>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
        }
    }
}
