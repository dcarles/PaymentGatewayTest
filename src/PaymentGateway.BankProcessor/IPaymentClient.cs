using System.Threading.Tasks;
using PaymentGateway.BankProcessor.Models;

namespace PaymentGateway.BankProcessor
{
    public interface IPaymentClient
    {
        Task<BankPaymentResponse> RequestAsync(BankPaymentRequest bankPaymentRequest);
    }
}
