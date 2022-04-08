using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationServices.Accounts.Response
{
    public class AccountViewModel
    {
        public string AccountNo { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; } 
        public decimal AvailableBalance { get; set; } 
        public decimal LedgerBalance { get; set; }
        public AccountType AccountType { get; set; }
    }
}
