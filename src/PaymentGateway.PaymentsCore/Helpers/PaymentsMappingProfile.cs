using AutoMapper;
using PaymentGateway.BankProcessor.Models;
using PaymentGateway.Data.Entities;
using PaymentGateway.PaymentsCore.Helpers;

namespace PaymentGateway.PaymentsCore
{
    public class PaymentsMappingProfile : Profile
    {
        public PaymentsMappingProfile()
        {
            CreateMap<Transaction, PaymentDetails>().ForMember(t => t.PaymentId, opt => opt.MapFrom(src => src.TransactionId))
                .ForMember(t => t.CardNumber,
                    opt => opt.MapFrom(src => MaskCardNumber(src.CardNumber)))
                .ForMember(t => t.Status,
                    opt => opt.MapFrom(src => ToStatusCode(src.Status)));

            CreateMap<CardPayment, PaymentProcessingRequest>();
            CreateMap<Transaction, CardPayment>();
            CreateMap<CardPayment, Transaction>().ForMember(opt => opt.CardNumber,
                src => src.MapFrom(o => NormalizeCardNumber(o.CardNumber)));

        }

        private static string ToStatusCode(TransactionStatus status)
        {
            return status.GetDescription();
        }

        private static string MaskCardNumber(string cardNumber)
        {
            return string.Concat(
                "".PadLeft(12, 'X'),
                cardNumber.Substring(cardNumber.Length - 4));
        }

        private static string NormalizeCardNumber(string cardNumber)
        {
            if (cardNumber.Contains(" "))
                return cardNumber.Replace(" ", string.Empty);
            if (cardNumber.Contains('-'))
                return cardNumber.Replace("-", string.Empty);
            return cardNumber;
        }
    }
}