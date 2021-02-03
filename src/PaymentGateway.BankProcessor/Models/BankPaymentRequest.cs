using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PaymentGateway.BankProcessor.Models
{
    /// <summary>
    /// Defines the object that is passed to bank
    /// </summary>
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class BankPaymentRequest
    {
        /// <summary>
        /// 16 digit card number
        /// </summary>
        public string CardNumber { get; set; }

        /// <summary>
        /// I deliberately use a field combining expiry month and year, I assumed that bank asks in this format
        /// Format should be  "month/year", i.e. 03/2022 or 4/2020
        /// </summary>
        public string ExpiryDate { get; set; }

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