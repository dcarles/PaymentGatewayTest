using System;
using System.Threading.Tasks;
using AutoMapper;
using PaymentGateway.BankProcessor.Models;

namespace PaymentGateway.BankProcessor
{
    /// <summary>
    ///  Payment processor to process payment requests by using Acquiring Bank's API
    /// </summary>
    public class PaymentProcessor : IPaymentProcessor
    {
        private readonly IPaymentClient _paymentClient;
        private readonly IMapper _mapper;


        public PaymentProcessor(IPaymentClient paymentClient, IMapper mapper)
        {
            _paymentClient = paymentClient;
            _mapper = mapper;
        }


        /// <summary>
        /// Requests payment from acquiring Bank and returns the payment processing response
        /// </summary>
        /// <param name="paymentRequest"> <see cref="PaymentProcessingRequest"/> request object to be passed</param>
        /// <returns> <see cref="PaymentProcessingResponse"/> object showing the state of the payment</returns>
        public async Task<PaymentProcessingResponse> ProcessAsync(PaymentProcessingRequest paymentRequest)
        {
            if (paymentRequest == null)
                throw new ArgumentNullException(nameof(paymentRequest), "Payment processing request cannot be null");

            // Mapping processing request to bank payment request.
            var bankPaymentRequest = _mapper.Map<BankPaymentRequest>(paymentRequest);

            //Make client request
            var bankPaymentResponse = await _paymentClient.RequestAsync(bankPaymentRequest);

            if (bankPaymentResponse == null)
                throw new ArgumentNullException(nameof(paymentRequest), "Payment client returned null response");


            // Approved
            if (bankPaymentResponse.ResponseCode == BankResponseCode.Approved)
                return new PaymentProcessingResponse(bankPaymentResponse.ReferenceId,
                    true,
                    bankPaymentResponse.ResponseCode,
                    bankPaymentResponse.ResponseMessage);

            return new PaymentProcessingResponse(bankPaymentResponse.ReferenceId,
                false,
                bankPaymentResponse.ResponseCode,
                bankPaymentResponse.ResponseMessage);
        }

    }
}