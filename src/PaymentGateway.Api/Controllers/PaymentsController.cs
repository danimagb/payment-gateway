namespace PaymentGateway.Api.Controllers
{
    using MediatR;

    using Microsoft.AspNetCore.Mvc;

    using PaymentGateway.Application.Payments;
    using PaymentGateway.Application.Payments.Commands.Create;
    using PaymentGateway.Application.Payments.Queries.GetById;

    [ApiController]
    [Route("[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly ILogger<PaymentsController> logger;

        private IMediator mediator { get; }

        public PaymentsController(ILogger<PaymentsController> logger, IMediator mediator)
        {
            this.logger = logger;
            this.mediator = mediator;
        }

        [HttpPost(Name = "CreatePayment")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentDTO payment)
        {
            var paymentId = await mediator.Send(new CreatePaymentCommand(Guid.NewGuid(), payment));

            return Accepted($"/payments/{paymentId}", null);
        }

        [HttpGet("{id}", Name = "GetPaymentDetails")]
        [ProducesResponseType(typeof(PaymentDetailsDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaymentDetails([FromRoute] Guid id)
        {
            var payment = await mediator.Send(new GetPaymentByIdQuery(new Guid("72e07f57-eb9c-4bc3-862e-6c4416d5744d"), id));

            if(payment is null)
            {
                return NotFound();
            }

            return Ok(payment);
        }
    }
}