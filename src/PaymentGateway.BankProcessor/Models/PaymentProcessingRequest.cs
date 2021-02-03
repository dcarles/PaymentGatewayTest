using System;

namespace PaymentGateway.BankProcessor.Models
{
    /// <summary>
    /// Defines a request to be sent to 
    /// </summary>
    public class PaymentProcessingRequest
    {
        public Guid TransactionId { get; set; }
        public string CardNumber { get; set; }
        public string Cvv { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
    }
}
