using Hotel.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Review.Commands.DeleteReview
{
    public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand, int>
    {
        private readonly HotelDbContext _context;

        public DeleteReviewCommandHandler(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(DeleteReviewCommand request, CancellationToken ct)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == request.Id, ct);

            if (review == null)
                throw new KeyNotFoundException($"Review with ID {request.Id} not found.");

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync(ct);

            return 1;
        }
    }
}
