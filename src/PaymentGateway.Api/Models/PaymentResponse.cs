using System;

namespace PaymentGateway.Api.Models
{
    /// <summary>
    /// Defines the response to client
    /// </summary>
    public class PaymentResponse
    {
        public Guid PaymentId { get; set; }
        public string CardNumber { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime CreatedOn { get; set; }
        public string StatusCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
