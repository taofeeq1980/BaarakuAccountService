﻿
using ApplicationServices.Accounts.Command;
using ApplicationServices.Interfaces;
using ApplicationServices.Shared.BaseResponse;
using ApplicationServices.Shared.Utilities;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;


namespace ApplicationServices.Accounts.Commandhandler
{
    public class AddAccountCommandHandler : IRequestHandler<AddAccountCommand, Result>
    {
        private readonly ILogger<AddAccountCommandHandler> _logger;
        private readonly IAccountServiceDbContext _context;
        private readonly IHostingEnvironment _env;
        public AddAccountCommandHandler(ILogger<AddAccountCommandHandler> logger, IAccountServiceDbContext context, IHostingEnvironment env)
        {
            _logger = logger;
            _env = env;
            _context = context;
        }
        public async Task<Result> Handle(AddAccountCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating Account...");

            var customer = Customer.Create(request.Firstname, request.Lastname, request.Email, request.PhoneNo);
            if (request.Image is not null)
            {
                customer.Image = Util.SaveImageAndReturnFilepath(request.Image, request.MimeType, _env); ;
            }
            await _context.Customers.AddAsync(customer, cancellationToken);
            if (await _context.SaveChangesAsync(cancellationToken) > 0)
            {
                var account = Account.Create(customer.Id, request.AccountType);
                account.AccountNo = $"{(int)request.AccountType}{Util.RandomDigits()}";
                account.SetAvailableBalance(0);
                account.SetLedgerBalance(0);
                await _context.Accounts.AddAsync(account, cancellationToken);
                int response = await _context.SaveChangesAsync(cancellationToken);
                return response > 0 ? Result.Ok("Account Creation Successful") : Result.Fail("Account Creation Failed");
            }
            return Result.Fail("Account Creation Failed");
        }

    }
}
