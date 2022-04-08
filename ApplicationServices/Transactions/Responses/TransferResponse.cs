using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationServices.Transactions.Responses
{
    public class TransData
    {
        [JsonProperty("reference")]
        public string Reference { get; set; }

        [JsonProperty("integration")]
        public int Integration { get; set; }

        [JsonProperty("domain")]
        public string Domain { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("recipient")]
        public int Recipient { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("transfer_code")]
        public string TransferCode { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }

    public class TransferResponse
    {
        [JsonProperty("status")]
        public bool Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public TransData Data { get; set; }
    }
}
