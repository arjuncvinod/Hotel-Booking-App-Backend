using Hotel.Application.Booking.Commands.CreateBooking;
using Hotel.Application.Booking.Commands.DeleteBooking;
using Hotel.Application.Booking.Commands.UpdateBooking;
using Hotel.Application.Booking.Queries.GetBookingById;
using Hotel.Application.Booking.Queries.GetBookings;
using Hotel.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<BookingDto>>> GetAll()
        {
            GetBookingsQuery query = new GetBookingsQuery();
            var result = await _mediator.Send(query);
            return result is not null ? Ok(result) : NotFound();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDto>> GetById(int id)
        {
            GetBookingByIdQuery query = new GetBookingByIdQuery();
            query.Id = id;
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<BookingDto>> AddBooking([FromBody] CreateBookingCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BookingDto>> UpdateBooking(int id, [FromBody] UpdateBookingCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return result is not null ? Ok(result) : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var command = new DeleteBookingCommand();
            command.Id = id;
            var result = await _mediator.Send(command);
            return Ok("Booking Deleted");
        }
    }
}
