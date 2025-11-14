using Hotel.Application.DTOs;
using Hotel.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Application.Review.Commands.UpdateReview
{
    public class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand, ReviewDto>
    {
        private readonly HotelDbContext _context;

        public UpdateReviewCommandHandler(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<ReviewDto> Handle(UpdateReviewCommand request, CancellationToken ct)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == request.Id, ct);

            if (review == null)
                throw new KeyNotFoundException($"Review with ID {request.Id} not found.");

            review.HotelId = request.HotelId;
            review.CustomerId = request.CustomerId;
            review.Rating = request.Rating;
            if (!string.IsNullOrWhiteSpace(request.Comment))
                review.Comment = request.Comment;
            review.ReviewDate = request.ReviewDate;

            await _context.SaveChangesAsync(ct);

            return new ReviewDto
            {
                Id = review.Id,
                HotelId = review.HotelId,
                CustomerId = review.CustomerId,
                Rating = review.Rating,
                Comment = review.Comment,
                ReviewDate = review.ReviewDate
            };
        }
    }
}
