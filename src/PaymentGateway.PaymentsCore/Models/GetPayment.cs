using System;
using MediatR;

namespace PaymentGateway.PaymentsCore
{
    /// <summary>
    /// Defines payment details command
    /// </summary>
    public class GetPayment : IRequest<PaymentDetails>
    {

        public GetPayment(Guid transactionId, int merchantId)
        {
            TransactionId = transactionId;
            MerchantId = merchantId;
        }
        public Guid TransactionId { get; }
        public int MerchantId { get; }
    }
}
