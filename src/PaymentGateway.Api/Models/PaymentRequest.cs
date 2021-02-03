using System;

namespace PaymentGateway.Api.Models
{
    /// <summary>
    /// Transaction Request class. An instance of this class represents a payment payload
    /// </summary>
    [Serializable]
    public class PaymentRequest
    {
        /// <summary>
        /// 16 digit card number
        /// </summary>
        public string CardNumber { get; set; }
        /// <summary>
        /// The expiry month of the card
        /// i.e. 1...12
        /// </summary>
        public int ExpiryMonth { get; set; }
        /// <summary>
        /// The four-digit expiry year of the card.
        /// </summary>
        public int ExpiryYear { get; set; }
        /// <summary>
        /// The card verification value/code.
        /// 3 Digits, except for Amex which is 4
        /// </summary>
        public string Cvv { get; set; }
        /// <summary>
        /// The payment amount in the major currency.
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// Three-letter ISO currency code representing the currency of the payment.
        /// </summary>
        public string Currency { get; set; }
    }



}
