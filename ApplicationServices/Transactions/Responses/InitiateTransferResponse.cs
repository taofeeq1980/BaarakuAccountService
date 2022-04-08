using Newtonsoft.Json;
using System;

namespace ApplicationServices.Transactions.Responses
{
    public class InitiateTransferResponse
    {
        [JsonProperty("status")]
        public bool Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public TransferResponseData Data { get; set; }
    }
    public class TransferResponseData
    {
        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("domain")]
        public string Domain { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("integration")]
        public int Integration { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("recipient_code")]
        public string RecipientCode { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("is_deleted")]
        public bool IsDeleted { get; set; }

        [JsonProperty("details")]
        public TransferResponseDetails Details { get; set; }
    }
    public class TransferResponseDetails
    {
        [JsonProperty("authorization_code")]
        public object AuthorizationCode { get; set; }

        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }

        [JsonProperty("account_name")]
        public object AccountName { get; set; }

        [JsonProperty("bank_code")]
        public string BankCode { get; set; }

        [JsonProperty("bank_name")]
        public string BankName { get; set; }
    }
}
