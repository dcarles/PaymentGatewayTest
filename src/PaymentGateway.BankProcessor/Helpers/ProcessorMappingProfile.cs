using AutoMapper;
using PaymentGateway.BankProcessor.Models;

namespace PaymentGateway.BankProcessor.Helpers
{
    public class BankProcessorMappingProfile : Profile
    {
        public BankProcessorMappingProfile()
        {
            CreateMap<PaymentProcessingRequest, BankPaymentRequest>().ForMember(opt => opt.ExpiryDate,
                src => src.MapFrom(req => $"{req.ExpiryMonth}/{req.ExpiryYear}"));
        }
    }
}
