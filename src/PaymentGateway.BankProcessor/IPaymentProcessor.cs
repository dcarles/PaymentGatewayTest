using System.Threading.Tasks;
using PaymentGateway.BankProcessor.Models;

namespace PaymentGateway.BankProcessor
{
    public interface IPaymentProcessor
    {
        /// <summary>
        /// Interface to payment processor which processes payments via client
        /// </summary>
        /// <param name="paymentRequest"></param>
        /// <returns></returns>
        Task<PaymentProcessingResponse> ProcessAsync(PaymentProcessingRequest paymentRequest);
    }
}
