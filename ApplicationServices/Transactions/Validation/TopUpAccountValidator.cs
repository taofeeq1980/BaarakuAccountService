using ApplicationServices.Interfaces;
using ApplicationServices.Transactions.Command;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationServices.Transactions.Validation
{
    public class TopUpAccountValidator : AbstractValidator<TopUpAccountCommand>
    {
        private readonly IAccountServiceDbContext _context;
        public TopUpAccountValidator(IAccountServiceDbContext context) 
        {
            _context = context;

            RuleFor(a => a.AccountNo).NotNull().NotEmpty().WithMessage("AccountNo cannot be empty");
            RuleFor(a => a.Amount).GreaterThan(0).WithMessage("Amount value should be greater than zero");
            RuleFor(a => a.Narration).NotNull().NotEmpty().WithMessage("Narration cannot be empty");
            RuleFor(a => a.AccountNo).MustAsync(AccountNoMustExist).WithMessage("Account No record do not exist");
        }

        private async Task<bool> AccountNoMustExist(string param, CancellationToken cancellationToken) 
        {
            return await _context.Accounts.AnyAsync(a => a.AccountNo == param, cancellationToken);
        }
    }
}