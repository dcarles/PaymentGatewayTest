namespace PaymentGateway.BankProcessor.Simulator
{
    /// <summary>
    /// Defines a re
    /// </summary>
    public class PaymentRequest
    {
        /// <summary>
        /// 16 digit card number
        /// </summary>
        public string CardNumber { get; set; }

        /// <summary>
        /// Card Expiry Date
        /// </summary>
        public string ExpiryDate { get; set; }

        /// <summary>
        /// The card verification value/code.
        /// 3 Digits, except for Amex which is 4
        /// </summary>
        public string Cvv { get; set; }

        /// <summary>
        /// The payment amount 
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Three-letter ISO currency code
        /// </summary>
        public string Currency { get; set; }
    }
}