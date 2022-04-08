using ApplicationServices.Interfaces;
using ApplicationServices.Shared.BaseResponse;
using ApplicationServices.Shared.Helpers;
using ApplicationServices.Shared.Utilities;
using ApplicationServices.Transactions.Command;
using ApplicationServices.Transactions.Responses;
using ApplicationServices.Transactions.ViewModel;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationServices.Transactions.CommandHandler
{
    public class FundTransferCommandHandler : PaystackAdapter, IRequestHandler<FundTransferCommand, Result>
    {
        private readonly IConfiguration _config;
        private readonly ILogger<FundTransferCommandHandler> _logger;
        private readonly IAccountServiceDbContext _context;

        public FundTransferCommandHandler(IConfiguration config,
            ILogger<FundTransferCommandHandler> logger,
            IAccountServiceDbContext context) : base(config, logger)
        {
            _config = config;
            _logger = logger;
            _context = context;
        }

        public async Task<Result> Handle(FundTransferCommand request, CancellationToken cancellationToken)
        {
            //debit account.
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNo == request.SourceAccountNo, cancellationToken);
            if (account.AvailableBalance < request.Amount) return Result.Fail("Fund Transfer Failed, Insufficient Balance");

            account.SetAvailableBalance(-request.Amount);
            _context.Accounts.Update(account);
            if (await _context.SaveChangesAsync(cancellationToken) <= 0)
            {
                Result.Fail("Fund Transfer Failed");
            }
            //log the transactions
            var transaction = new Transaction
            {
                AccountId = account.Id,
                Narration = request.Narration,
                Amount = request.Amount,
                TransactionReference = Util.RandomDigits(12),
                TransactionType = Domain.TransactionType.FundTransfer,
                Status = Domain.TransactionStatus.Pending
            };
            await _context.Transactions.AddAsync(transaction, cancellationToken);
            if (await _context.SaveChangesAsync(cancellationToken) <= 0)
            {
                account.SetAvailableBalance(request.Amount);
                _context.Accounts.Update(account);
                await _context.SaveChangesAsync(cancellationToken);
                Result.Fail("Fund Transfer Failed");
            }
            //call initiate transfer
            var model = new InitiateTransferRequest
            {
                Account_Number = request.DestinationAccountNo,
                Amount = request.Amount.ToString("N0"),
                Bank_Code = request.BankCode,
                Name = request.DestinationAccountName,
                Narration = request.Narration
            };
            var result = await PostResponse<InitiateTransferResponse>(_config["PayStackSettings:IntitiateTransfer"], model);
            if (result == null || !result.Status)
            {
                account.SetAvailableBalance(request.Amount);

                await ReversenUpdateRecord(transaction, account, request, cancellationToken,
                null, null, _context, Domain.TransactionStatus.Failed);

                return result == null ? Result.Fail("Fund Transfer Failed") : Result.Fail(result.Message);
            }
            //usw the transfer to call transfer 
            var command = new TransferRequest
            {
                Amount = request.Amount.ToString("N0"),
                Reason = request.Narration,
                Recipient = result.Data.RecipientCode
            };
            var transferresult = await PostResponse<TransferResponse>(_config["PayStackSettings:TransferURL"], command);
            if (transferresult == null)
            {
                account.SetAvailableBalance(request.Amount);

                await ReversenUpdateRecord(transaction, account, request, cancellationToken,
                   null, command.Recipient, _context, Domain.TransactionStatus.Failed);

                return Result.Fail("Fund Transfer Failed");
            }
            if (!transferresult.Status)
            {
                account.SetAvailableBalance(request.Amount);
                await ReversenUpdateRecord(transaction, account, request, cancellationToken,
                    JsonConvert.SerializeObject(transferresult.Data), command.Recipient,
                    _context, Domain.TransactionStatus.Failed);

                return Result.Fail(transferresult.Message);
            }
            //update payment gateway response
            account.SetLedgerBalance(-request.Amount);
            await ReversenUpdateRecord(transaction, account, request, cancellationToken,
                    JsonConvert.SerializeObject(transferresult.Data), transferresult.Data.Recipient.ToString(),
                    _context, Domain.TransactionStatus.Successful);

            return Result.Ok(transferresult.Message);

            // return Response<GenericResponse>.Success(new GenericResponse { Data = transferresult.Data }, transferresult.Message);
        }

        private static async Task ReversenUpdateRecord(Transaction transaction, Account account, FundTransferCommand request,
                CancellationToken cancellationToken, string PaymentGatewayResponse, string PaymentGatewayTransferCode,
                IAccountServiceDbContext _context, Domain.TransactionStatus transactionStatus)
        {

            transaction.Status = transactionStatus;
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync(cancellationToken);

            transaction.BeneficiaryAccountName = request.DestinationAccountName;
            transaction.BeneficiaryAccountNo = request.DestinationAccountNo;
            transaction.BankCode = request.BankCode;
            transaction.PaymentGatewayResponse = PaymentGatewayResponse;
            transaction.PaymentGatewayTransferCode = PaymentGatewayTransferCode;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
