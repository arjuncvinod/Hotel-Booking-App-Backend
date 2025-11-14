using Hotel.Application.DTOs;
using Hotel.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Review.Queries.GetReviews
{
    public class GetReviewsQueryHandler : IRequestHandler<GetReviewsQuery, List<ReviewDto>>
    {
        private readonly HotelDbContext _context;

        public GetReviewsQueryHandler(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<List<ReviewDto>> Handle(GetReviewsQuery request, CancellationToken cancellationToken)
        {
            var reviews = await _context.Reviews.ToListAsync(cancellationToken);

            var reviewDtos = new List<ReviewDto>();

            foreach (var review in reviews)
            {
                var reviewDto = new ReviewDto
                {
                    Id = review.Id,
                    HotelId = review.HotelId,
                    CustomerId = review.CustomerId,
                    Rating = review.Rating,
                    Comment = review.Comment,
                    ReviewDate = review.ReviewDate
                };

                reviewDtos.Add(reviewDto);
            }

            return reviewDtos;
        }
    }
}
