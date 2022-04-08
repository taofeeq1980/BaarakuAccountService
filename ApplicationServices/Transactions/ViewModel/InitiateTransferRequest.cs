namespace ApplicationServices.Transactions.ViewModel
{
    public class InitiateTransferRequest
    {
        public string Type { get; } = "nuban";
        public string Name { get; set; }
        public string Account_Number { get; set; }
        public string Bank_Code { get; set; }
        public string Currency { get; } = "NGN";
        public string Amount { get; set; }
        public string Narration { get; set; }
    }
}
