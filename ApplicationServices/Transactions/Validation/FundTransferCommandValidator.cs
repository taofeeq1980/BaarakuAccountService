using ApplicationServices.Interfaces;
using ApplicationServices.Transactions.Command;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationServices.Transactions.Validation
{
    public class FundTransferCommandValidator : AbstractValidator<FundTransferCommand>
    {
        private readonly IAccountServiceDbContext _context;
        public FundTransferCommandValidator(IAccountServiceDbContext context)
        {
            _context = context;

            RuleFor(a => a.SourceAccountNo).NotNull().NotEmpty().WithMessage("SourceAccountNo cannot be empty");
            RuleFor(a => a.DestinationAccountName).NotNull().NotEmpty().WithMessage("DestinationAccountName cannot be empty");
            RuleFor(a => a.DestinationAccountNo).NotNull().NotEmpty().WithMessage("DestinationAccountNo cannot be empty");
            RuleFor(a => a.Amount).GreaterThan(0).WithMessage("Amount value should be greater than zero");
            RuleFor(a => a.Narration).NotNull().NotEmpty().WithMessage("Narration cannot be empty");
            RuleFor(a => a.SourceAccountNo).MustAsync(AccountNoMustExist).WithMessage("Account No record do not exist");
        }

        private async Task<bool> AccountNoMustExist(string param, CancellationToken cancellationToken)
        {
            return await _context.Accounts.AnyAsync(a => a.AccountNo == param, cancellationToken);
        }
    }
}