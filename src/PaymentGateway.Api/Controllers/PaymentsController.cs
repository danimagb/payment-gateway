namespace PaymentGateway.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using PaymentGateway.Application.Payments;

    [ApiController]
    [Route("[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(ILogger<PaymentsController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "CreatePayment")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public IActionResult CreatePayment()
        {
            return Accepted($"/payments/{Guid.NewGuid()}", null);
        }

        [HttpGet("{id}",Name = "GetPaymentDetails")]
        [ProducesResponseType(typeof(PaymentDetailsDTO), StatusCodes.Status200OK)]
        public IActionResult GetPaymentDetails([FromRoute] Guid id)
        {
            return Ok(new PaymentDetailsDTO
            {
                Amount = 1000,
                Currency = "EUR",
                CardDetails = new CardDetailsDTO
                {
                    CardNumber = "1212121",
                    Cvv = 223,
                    ExpiryMonth = 2,
                    ExpiryYear = 2030
                },
                Status = "Accepted"
            });
        }
    }
}