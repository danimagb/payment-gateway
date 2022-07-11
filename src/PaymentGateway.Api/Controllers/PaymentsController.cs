namespace PaymentGateway.Api.Controllers
{
    using MediatR;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using PaymentGateway.Application.Payments;
    using PaymentGateway.Application.Payments.Commands.Create;
    using PaymentGateway.Application.Payments.Queries.GetById;

    
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private IMediator mediator { get; }

        public PaymentsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost(Name = "CreatePayment")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentRequestDTO payment, CancellationToken cancellationToken)
        {
            var paymentDetails = await mediator.Send(new CreatePaymentCommand(this.GetAuthenticatedMerchantId(), payment), cancellationToken);

            return CreatedAtRoute(nameof(GetPaymentDetails), new { id = paymentDetails.Id }, paymentDetails);
        }

        [HttpGet("{id:guid}", Name = "GetPaymentDetails")]
        [ProducesResponseType(typeof(PaymentDetailsResponseDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaymentDetails([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var payment = await mediator.Send(new GetPaymentByIdQuery(this.GetAuthenticatedMerchantId(), id), cancellationToken);

            if(payment is null)
            {
                return NotFound();
            }

            return Ok(payment);
        }
    }
}