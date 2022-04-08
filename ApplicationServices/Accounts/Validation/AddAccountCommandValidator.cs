using ApplicationServices.Accounts.Command;
using ApplicationServices.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationServices.Accounts.Validation
{
    public class AddAccountCommandValidator : AbstractValidator<AddAccountCommand>
    {
        private readonly IAccountServiceDbContext _context;
        public AddAccountCommandValidator(IAccountServiceDbContext context)
        {
            _context = context;

            RuleFor(a => a.Firstname).NotNull().NotEmpty().WithMessage("Firstname cannot be empty");
            RuleFor(a => a.Lastname).NotNull().NotEmpty().WithMessage("Lastname cannot be empty");
            RuleFor(a => a.PhoneNo).NotNull().NotEmpty().WithMessage("PhoneNo cannot be empty");
            RuleFor(a => a.Email).NotNull().NotEmpty().WithMessage("Email cannot be empty");
            RuleFor(a => a.Email).MustAsync(CustomerMustNotExist).WithMessage("Email record exist");
            RuleFor(a => a.PhoneNo).MustAsync(CustomerMustNotExist).WithMessage("Phone No record exist");
        }

        private async Task<bool> CustomerMustNotExist(string param, CancellationToken cancellationToken) 
        {
            return !await _context.Customers.AnyAsync(a => a.Email == param || a.PhoneNo == param, cancellationToken);   
        }
    }
}