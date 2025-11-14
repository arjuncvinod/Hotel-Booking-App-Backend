using Hotel.Application.DTOs;
using Hotel.Application.RoomType.Command.CreateRoomType;
using Hotel.Application.RoomType.Command.DeleteRoomType;
using Hotel.Application.RoomType.Command.UpdateRoomType;
using Hotel.Application.RoomType.Queries.GetRoomTypeById;
using Hotel.Application.RoomType.Queries.GetRoomTypes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomTypeController : ControllerBase
    {

        private readonly IMediator _mediator;
        public RoomTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<RoomTypeDto>> Add(CreateRoomTypeCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet]
        public async Task<ActionResult<List<RoomTypeDto>>> GetAll()
        {
            var query = new GetRoomTypesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoomTypeDto>> GetById(int id)
        {
           GetRoomTypeByIdQuery query = new GetRoomTypeByIdQuery();
            query.Id = id;
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<RoomTypeDto>> UpdateRoomType(int id, [FromBody] UpdateRoomTypeCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return result is not null ? Ok(result) : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var command = new DeleteRoomTypeCommand();
            command.Id = id;
            var result = await _mediator.Send(command);
            return Ok("RoomType Deleted");
        }
    }
}
