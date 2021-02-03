using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentGateway.Data.Entities
{
    /// <summary>
    /// Entity defining  a payment. A new transaction is created when a new payment requested
    /// </summary>
    public class Transaction : BaseEntity
    {
        /// <summary>
        /// Unique id of transaction
        /// </summary>
        [Key]
        public Guid TransactionId { get; set; }

        /// <summary>
        /// Utc format
        /// </summary>
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        /// <summary>
        /// Encrypted card number
        /// </summary>
        public string CardNumber { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string Cvv { get; set; }
        [Column(TypeName = "Money")]
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public TransactionStatus Status { get; set; } = TransactionStatus.Pending;
        public string ErrorMessage { get; set; }
        public int MerchantId { get; set; }
        public Merchant Merchant { get; set; }
        public string BankReferenceId { get; set; }

    }

    public enum TransactionStatus
    {
        [Description("Approved")]
        Approved = 1,
        [Description("Declined")]
        Declined = 2,
        [Description("Pending")]
        Pending = 3
    }
}