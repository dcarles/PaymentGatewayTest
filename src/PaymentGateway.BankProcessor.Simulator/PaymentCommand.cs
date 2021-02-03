using MediatR;
using System;

namespace PaymentGateway.BankProcessor.Simulator
{
    public class PaymentCommand : IRequest<PaymentResponse>
    {
        public PaymentCommand(string cardNumber, string expiryDate, string cvv, decimal amount, string currency)
        {
            CardNumber = cardNumber;
            ExpiryDate = expiryDate;
            Cvv = cvv;
            Amount = amount;
            Currency = currency;
            // in a proper application this can be generated in more sophisticated way
            PaymentRequestId = $"pay_{Guid.NewGuid()}";
        }

        /// <summary>
        /// My Bank Simulator use different format of unique Id
        /// </summary>
        public string PaymentRequestId { get; set; }

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
