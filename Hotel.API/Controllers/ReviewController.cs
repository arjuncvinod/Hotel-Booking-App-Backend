using Hotel.Application.DTOs;
using Hotel.Application.Review.Commands.CreateReview;
using Hotel.Application.Review.Commands.DeleteReview;
using Hotel.Application.Review.Commands.UpdateReview;
using Hotel.Application.Review.Queries.GetReviewById;
using Hotel.Application.Review.Queries.GetReviews;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReviewController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<ReviewDto>>> GetAll()
        {
            GetReviewsQuery query = new GetReviewsQuery();
            var result = await _mediator.Send(query);
            return result is not null ? Ok(result) : NotFound();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewDto>> GetById(int id)
        {
            GetReviewByIdQuery query = new GetReviewByIdQuery();
            query.Id = id;
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ReviewDto>> AddReview([FromBody] CreateReviewCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ReviewDto>> UpdateReview(int id, [FromBody] UpdateReviewCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return result is not null ? Ok(result) : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var command = new DeleteReviewCommand();
            command.Id = id;
            var result = await _mediator.Send(command);
            return Ok("Review Deleted");
        }
    }
}
