using ApplicationServices.Shared.BaseResponse;
using MediatR;
using System.Text.Json.Serialization;

namespace ApplicationServices.Transactions.Command
{
    public class FundTransferCommand : IRequest<Result>
    {
        public string SourceAccountNo { get; set; }
        public string DestinationAccountNo { get; set; } 
        public string DestinationAccountName { get; set; } 
        public decimal Amount { get; set; }
        public string Narration { get; set; }
        public string BankCode { get; set; } 
    }
}
