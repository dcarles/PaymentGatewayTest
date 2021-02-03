using AutoMapper;
using PaymentGateway.Api.Models;
using PaymentGateway.PaymentsCore;

namespace PaymentGateway.Api.Core
{
    public class ApiMappingProfile : Profile
    {
        public ApiMappingProfile()
        {

            CreateMap<PaymentRequest, CardPayment>();
            CreateMap<PaymentDetails, PaymentResponse>();
        }
    }
}