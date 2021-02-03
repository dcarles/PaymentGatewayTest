namespace PaymentGateway.BankProcessor.Models
{
    public class PaymentProcessingResponse
    {
        public PaymentProcessingResponse(string bankReferenceId, bool approved, string responseCode, string responseMessage)
        {
            BankReferenceId = bankReferenceId;
            Approved = approved;
            ResponseCode = responseCode;
            ResponseMessage = responseMessage;
        }
        public string BankReferenceId { get; set; }
        public bool Approved { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
    }
}