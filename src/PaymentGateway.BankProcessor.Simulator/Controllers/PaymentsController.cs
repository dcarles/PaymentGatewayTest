using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace PaymentGateway.BankProcessor.Simulator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : Controller
    {
        private readonly IMediator _mediator;

        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }


        // GET: api/<controller> -- NOT IMPLEMENTED
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Api is working fine...");
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PaymentRequest request)
        {
            // Compose Command
            var requestCommand = new PaymentCommand(request.CardNumber,
                request.ExpiryDate, request.Cvv, request.Amount, request.Currency);
            var paymentResponse = await _mediator.Send(requestCommand);

            //1000 means Payment Accepted, 2000 means payment Declined
            if (paymentResponse.ResponseCode == "1000")
                return Accepted(paymentResponse);
            return BadRequest(paymentResponse);
        }
    }
}
