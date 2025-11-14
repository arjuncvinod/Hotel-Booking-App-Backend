using Hotel.Application.DTOs;
using Hotel.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Booking.Queries.GetBookings
{
    public class GetBookingsQueryHandler : IRequestHandler<GetBookingsQuery, List<BookingDto>>
    {
        private readonly HotelDbContext _context;

        public GetBookingsQueryHandler(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<List<BookingDto>> Handle(GetBookingsQuery request, CancellationToken cancellationToken)
        {
            var bookings = await _context.Bookings.ToListAsync(cancellationToken);

            var bookingDtos = new List<BookingDto>();

            foreach (var booking in bookings)
            {
                var bookingDto = new BookingDto
                {
                    Id = booking.Id,
                    CustomerId = booking.CustomerId,
                    RoomId = booking.RoomId,
                    CheckInDate = booking.CheckInDate,
                    CheckOutDate = booking.CheckOutDate,
                    Status = booking.Status,
                    TotalAmount = booking.TotalAmount
                };

                bookingDtos.Add(bookingDto);
            }

            return bookingDtos;
        }
    }
}
