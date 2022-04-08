using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationServices.Transactions.ViewModel
{
    public class TransferRequest
    {
        public string Source { get; } = "balance";
        public string Amount { get; set; }
        public string Recipient { get; set; }
        public string Reason { get; set; }
    }
}
