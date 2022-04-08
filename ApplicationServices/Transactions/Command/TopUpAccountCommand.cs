using ApplicationServices.Shared.BaseResponse;
using Domain;
using MediatR;
using System.Text.Json.Serialization;

namespace ApplicationServices.Transactions.Command 
{
    public class TopUpAccountCommand : IRequest<Result>
    {
        public string AccountNo { get; set; }
        public decimal Amount { get; set; }
        public string Narration { get; set; }
        [JsonIgnore]
        public TransactionType TransactionType { get { return TransactionType.TopUp; } }
    }
}
