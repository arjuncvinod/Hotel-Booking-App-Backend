using Hotel.Application.DTOs;
using Hotel.Application.Payment.Commands.CreatePayment;
using Hotel.Application.Payment.Commands.DeletePayment;
using Hotel.Application.Payment.Commands.UpdatePayment;
using Hotel.Application.Payment.Queries.GetPaymentById;
using Hotel.Application.Payment.Queries.GetPayments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<PaymentDto>>> GetAll()
        {
            GetPaymentsQuery query = new GetPaymentsQuery();
            var result = await _mediator.Send(query);
            return result is not null ? Ok(result) : NotFound();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentDto>> GetById(int id)
        {
            GetPaymentByIdQuery query = new GetPaymentByIdQuery();
            query.Id = id;
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<PaymentDto>> AddPayment([FromBody] CreatePaymentCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PaymentDto>> UpdatePayment(int id, [FromBody] UpdatePaymentCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return result is not null ? Ok(result) : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var command = new DeletePaymentCommand();
            command.Id = id;
            var result = await _mediator.Send(command);
            return Ok("Payment Deleted");
        }
    }
}
