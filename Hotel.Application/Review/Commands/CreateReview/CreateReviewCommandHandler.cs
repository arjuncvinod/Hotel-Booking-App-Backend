using Hotel.Application.DTOs;
using Hotel.Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Review.Commands.CreateReview
{
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, ReviewDto>
    {
        private readonly HotelDbContext _context;

        public CreateReviewCommandHandler(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<ReviewDto> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            var review = new Domain.Entities.Review
            {
                HotelId = request.HotelId,
                CustomerId = request.CustomerId,
                Rating = request.Rating,
                Comment = request.Comment,
                ReviewDate = request.ReviewDate
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync(cancellationToken);

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
