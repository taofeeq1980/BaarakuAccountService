using FluentValidation;

namespace ApplicationServices.Login.Validation
{
    public class TokenCommandValidator : AbstractValidator<ConnectTokenCommand>
    {      
        public TokenCommandValidator()
        {           
            RuleFor(a => a.Username).NotNull().NotEmpty().WithMessage("Username cannot be empty");
            RuleFor(a => a.Password).NotNull().NotEmpty().WithMessage("Password cannot be empty");       
        }
    }
}
