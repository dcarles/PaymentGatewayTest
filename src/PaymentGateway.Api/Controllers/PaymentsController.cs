using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.Api.Models;
using PaymentGateway.PaymentsCore;

namespace PaymentGateway.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {

        private readonly ILogger<PaymentsController> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public PaymentsController(ILogger<PaymentsController> logger, IMediator mediator, IMapper mapper)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;

        }

        [HttpGet("{id:guid}", Name = "Get")]
        public async Task<IActionResult> Get(string id)
        {
            var getPayment = new GetPayment(Guid.Parse(id), int.Parse(User.Identity.Name));
            var paymentDetails = await _mediator.Send(getPayment);
            if (paymentDetails == null)
                return NotFound("Payment does not exist");

            var paymentResponse = _mapper.Map<PaymentResponse>(paymentDetails);
            return Ok(paymentResponse);
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PaymentRequest paymentRequest)
        {
            var payment = _mapper.Map<CardPayment>(paymentRequest);
            payment.MerchantId = int.Parse(User.Identity.Name); ;
            var paymentResult = await _mediator.Send(payment);


            switch (paymentResult)
            {
                case PaymentResult.Success _:
                    var paymentUrl = $"{HttpContext.Request.GetEncodedUrl()}/{paymentResult.GatewayReferenceId}";
                    return Created(paymentUrl, paymentResult);
                case PaymentResult.Declined _:
                    return BadRequest(paymentResult);
                default:
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
