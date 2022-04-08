using System;

namespace Domain.Entities
{
    public class Transaction : Entity
    {
        public TransactionType TransactionType { get; set; }    
        public string TransactionReference { get; set; }
        public string Narration { get; set; }
        public TransactionStatus Status { get; set; }
        public decimal Amount { get; set; }
        public Guid AccountId { get; set; } 
        public Account Account { get; set; } 

        public string BeneficiaryAccountNo { get; set; } 
        public string BeneficiaryAccountName { get; set; } 
        public string BankCode { get; set; }
        public string PaymentGatewayTransferCode { get; set; }
        public string PaymentGatewayResponse { get; set; }
    }
}
