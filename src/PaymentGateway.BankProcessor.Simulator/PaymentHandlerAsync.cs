using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentGateway.BankProcessor.Simulator
{
    public class PaymentHandlerAsync : IRequestHandler<PaymentCommand, PaymentResponse>
    {
        public async Task<PaymentResponse> Handle(PaymentCommand request, CancellationToken cancellationToken)
        {
            var cardResponse = MockData.ValidateCard(request.CardNumber, request.Cvv);
            return new PaymentResponse()
            {
                ReferenceId = $"pay_{Guid.NewGuid()}",
                ResponseCode = cardResponse.ResponseCode,
                ResponseMessage = cardResponse.ResponseMessage,
                ProcessedOn = DateTime.UtcNow
            };
        }
    }
}
