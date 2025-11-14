using Hotel.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Booking.Commands.DeleteBooking
{
    public class DeleteBookingCommandHandler : IRequestHandler<DeleteBookingCommand, int>
    {
        private readonly HotelDbContext _context;

        public DeleteBookingCommandHandler(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(DeleteBookingCommand request, CancellationToken ct)
        {
            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == request.Id, ct);

            if (booking == null)
                throw new KeyNotFoundException($"Booking with ID {request.Id} not found.");

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync(ct);

            return 1;
        }
    }
}
