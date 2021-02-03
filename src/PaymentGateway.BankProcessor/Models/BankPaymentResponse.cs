using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PaymentGateway.BankProcessor.Models
{
    /// <summary>
    /// Defines bank payment response
    /// </summary>
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]

    public class BankPaymentResponse
    {
        /// <summary>
        /// A unique identifier that bank returns.
        /// A scenario is to query a transaction to know more about from bank
        /// </summary>
        public string ReferenceId { get; set; }
        /// <summary>
        /// Response codes provided by bank
        /// </summary>
        public string ResponseCode { get; set; }

        /// <summary>
        /// A brief description from bank, for example, if it failed, why?
        /// </summary>
        public string ResponseMessage { get; set; }


    }
}