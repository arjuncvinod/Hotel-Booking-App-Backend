using Hotel.Application.DTOs;
using Hotel.Application.Room.Commands.CreateRoom;
using Hotel.Application.Room.Commands.DeleteRoom;
using Hotel.Application.Room.Commands.UpdateRoom;
using Hotel.Application.Room.Queries.GetRoomById;
using Hotel.Application.Room.Queries.GetRooms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoomController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<RoomDto>>> GetAll()
        {
            GetRoomsQuery query = new GetRoomsQuery();
            var result = await _mediator.Send(query);
            return result is not null ? Ok(result) : NotFound();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<RoomDto>> GetById(int id)
        {
            GetRoomByIdQuery query = new GetRoomByIdQuery();
            query.Id = id;
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<RoomDto>> AddRoom([FromBody] CreateRoomCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<RoomDto>> UpdateRoom(int id, [FromBody] UpdateRoomCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return result is not null ? Ok(result) : NotFound();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var command = new DeleteRoomCommand();
            command.Id = id;
            bool result = await _mediator.Send(command);
            return result ? Ok(true) : NotFound();
        }
    }
}
