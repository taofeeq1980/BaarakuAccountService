using ApplicationServices.Interfaces;
using ApplicationServices.Shared.BaseResponse;
using ApplicationServices.Shared.Utilities;
using ApplicationServices.Transactions.Command;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationServices.Transactions.CommandHandler
{
    class TopUpAccountCommandHandler : IRequestHandler<TopUpAccountCommand, Result>
    {
        private readonly ILogger<TopUpAccountCommandHandler> _logger;
        private readonly IAccountServiceDbContext _context;

        public TopUpAccountCommandHandler(ILogger<TopUpAccountCommandHandler> logger, IAccountServiceDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<Result> Handle(TopUpAccountCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Top up Account...");
            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.AccountNo == request.AccountNo, cancellationToken);
            var transaction = new Transaction
            {
                AccountId = account.Id,
                Narration = request.Narration,
                Amount = request.Amount,
                TransactionReference = Util.RandomDigits(12),
                TransactionType = request.TransactionType
            };
            await _context.Transactions.AddAsync(transaction, cancellationToken);
            if (await _context.SaveChangesAsync(cancellationToken) > 0)
            {
                account.SetAvailableBalance(request.Amount);
                account.SetLedgerBalance(request.Amount);

                _context.Accounts.Update(account);
                int response = await _context.SaveChangesAsync(cancellationToken);
                return response > 0 ? Result.Ok("Account Top-up Successful") : Result.Fail("Account Top-up Failed");
            }

            return Result.Fail("Account Top-up Failed");
        }
    }
}
