using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using PaymentGateway.BankProcessor;
using PaymentGateway.BankProcessor.Models;
using PaymentGateway.Data;
using PaymentGateway.Data.Entities;

namespace PaymentGateway.PaymentsCore.Handlers
{
    public class CardPaymentHandler : IRequestHandler<CardPayment, PaymentResult>
    {
        private readonly IRepository<Transaction> _transactionRepository;
        private readonly IPaymentProcessor _paymentProcessor;
        private readonly IMapper _mapper;

        public CardPaymentHandler(IRepository<Transaction> transactionRepository,
            IPaymentProcessor paymentProcessor, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _paymentProcessor = paymentProcessor;
            _mapper = mapper;
        }

        public async Task<PaymentResult> Handle(CardPayment command, CancellationToken cancellationToken)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            // save transaction
            var transaction = _mapper.Map<Transaction>(command);
            await _transactionRepository.AddAsync(transaction);
            // request transaction via client
            // construct transaction processing request
            var paymentProcessingRequest = _mapper.Map<PaymentProcessingRequest>(command);
            var paymentProcessingResponse = await _paymentProcessor.ProcessAsync(paymentProcessingRequest);

            // Update transaction;
            await UpdateTransaction(transaction, paymentProcessingResponse);
            if (paymentProcessingResponse.Approved)
                return new PaymentResult.Success(command.TransactionId.ToString(),
                    paymentProcessingResponse.ResponseCode);
            return new PaymentResult.Declined(command.TransactionId.ToString(),
                paymentProcessingResponse.ResponseCode)
            {
                ResponseMessage = paymentProcessingResponse.ResponseMessage
            };
        }

        private async Task UpdateTransaction(Transaction transaction,
            PaymentProcessingResponse paymentProcessingResponse)
        {
            transaction.Status = paymentProcessingResponse.Approved
                ? TransactionStatus.Approved
                : TransactionStatus.Declined;
            transaction.BankReferenceId = paymentProcessingResponse.BankReferenceId;
            transaction.ErrorMessage = paymentProcessingResponse.ResponseMessage;
            await _transactionRepository.UpdateAsync(transaction);
        }
    }
}