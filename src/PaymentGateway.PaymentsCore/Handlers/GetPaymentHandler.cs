using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using PaymentGateway.Data;
using PaymentGateway.Data.Entities;

namespace PaymentGateway.PaymentsCore.Handlers
{
    public class GetPaymentHandler : IRequestHandler<GetPayment, PaymentDetails>
    {
        private readonly IRepository<Transaction> _transactionRepository;
        private readonly IMapper _mapper;
        public GetPaymentHandler(IRepository<Transaction> transactionRepository, IMapper _mapper)
        {
            _transactionRepository = transactionRepository;
            this._mapper = _mapper;
        }
        public async Task<PaymentDetails> Handle(GetPayment command, CancellationToken cancellationToken)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            var transaction = await _transactionRepository.GetSingleByQueryAsync(tr => tr.TransactionId == command.TransactionId
                                                                                      && tr.MerchantId == command.MerchantId);
            return _mapper.Map<PaymentDetails>(transaction);
        }
    }
}
