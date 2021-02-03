using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentGateway.Data.Entities
{
    /// <summary>
    /// Defines a merchant using payment gateway.
    /// </summary>
    public class Merchant : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MerchantId { get; set; }
        /// <summary>
        /// Merchant key to authorize the api calls
        /// </summary>
        public string ApiKey { get; set; }
        /// <summary>
        /// Email address of merchant.
        /// </summary>
        public string EmailAddress { get; set; }
    }
}