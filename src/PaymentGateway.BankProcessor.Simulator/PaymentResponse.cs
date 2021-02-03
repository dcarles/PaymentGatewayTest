using System;

namespace PaymentGateway.BankProcessor.Simulator
{
    public class PaymentResponse
    {
        public string ReferenceId { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public DateTime ProcessedOn { get; set; }
    }
}