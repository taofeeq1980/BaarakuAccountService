using AutoMapper;
using System;

namespace Domain.Entities
{
    public class Account : Entity
    {
        public Guid CustomerId { get; set; }
        public string AccountNo { get; set; }
        public AccountType AccountType { get; set; }
        public  decimal AvailableBalance { get; private set; }
        public  decimal LedgerBalance { get; private set; }

        public static Account Create(Guid customerId, AccountType accountType)
        {
            return new Account
            {
                CustomerId = customerId,
                AccountType = accountType
            };
        }

        public void SetAvailableBalance(decimal balance)
        {
            AvailableBalance += balance;
        }

        public void SetLedgerBalance(decimal balance)
        {
            LedgerBalance += balance;
        }
    }
}
