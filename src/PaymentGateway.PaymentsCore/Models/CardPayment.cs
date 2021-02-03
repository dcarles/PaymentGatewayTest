using System;
using MediatR;

namespace PaymentGateway.PaymentsCore
{
    public class CardPayment : IRequest<PaymentResult>
    {
        public CardPayment()
        {
            TransactionId = Guid.NewGuid();

        }

        /// <summary>
        /// Unique Identifier for the request
        /// </summary>
        public Guid TransactionId { get; }
        /// <summary>
        /// Unique id of merchant
        /// </summary>
        public int MerchantId { get; set; }

        /// <summary>
        /// 16 digit card number
        /// </summary>
        public string CardNumber { get; set; }
        /// <summary>
        /// The expiry month of the card
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
