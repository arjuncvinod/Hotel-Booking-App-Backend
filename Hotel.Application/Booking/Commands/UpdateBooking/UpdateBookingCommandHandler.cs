using Hotel.Application.DTOs;
using Hotel.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Application.Booking.Commands.UpdateBooking
{
    public class UpdateBookingCommandHandler : IRequestHandler<UpdateBookingCommand, BookingDto>
    {
        private readonly HotelDbContext _context;

        public UpdateBookingCommandHandler(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<BookingDto> Handle(UpdateBookingCommand request, CancellationToken ct)
        {
            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == request.Id, ct);

            if (booking == null)
                throw new KeyNotFoundException($"Booking with ID {request.Id} not found.");

            booking.CustomerId = request.CustomerId;
            booking.RoomId = request.RoomId;
            booking.CheckInDate = request.CheckInDate;
            booking.CheckOutDate = request.CheckOutDate;
            booking.Status = request.Status;
            booking.TotalAmount = request.TotalAmount;

            await _context.SaveChangesAsync(ct);

            return new BookingDto
            {
                Id = booking.Id,
                CustomerId = booking.CustomerId,
                RoomId = booking.RoomId,
                CheckInDate = booking.CheckInDate,
                CheckOutDate = booking.CheckOutDate,
                Status = booking.Status,
                TotalAmount = booking.TotalAmount
            };
        }
    }
}
