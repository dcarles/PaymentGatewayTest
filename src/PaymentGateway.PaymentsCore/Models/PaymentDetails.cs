using System;

namespace PaymentGateway.PaymentsCore
{
    public class PaymentDetails
    {
        public Guid PaymentId { get; set; }
        public string CardNumber { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string Cvv { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }

        /// <summary>
        ///     User friendly, sort of, code for the client to see when they query.
        /// </summary>
        public string Status { get; set; }
        public string ErrorMessage { get; set; }
        public int MerchantId { get; set; }
        public string BankReferenceId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}