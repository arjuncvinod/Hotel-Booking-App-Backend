using Hotel.Application.DTOs;
using Hotel.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Booking.Queries.GetBookingByCustomer
{
    public class GetBookingByCustomerQueryHandler:IRequestHandler<GetBookingByCustomerQuery, List<BookingDto>>

    {

        private readonly HotelDbContext _context;
        public GetBookingByCustomerQueryHandler( HotelDbContext context )
        {
            _context = context;
        }

        public async Task<List<BookingDto>> Handle(GetBookingByCustomerQuery request, CancellationToken cancellationToken)
        {
            var bookings = await _context.Bookings
                .AsNoTracking()                                   
                .Where(b => b.CustomerId == request.Id)           
                .OrderByDescending(b => b.CheckInDate)         
                .Select(b => new BookingDto
                {
                    Id = b.Id,
                    CustomerId = b.CustomerId,
                    RoomId = b.RoomId,
                    CheckInDate = b.CheckInDate,
                    CheckOutDate = b.CheckOutDate,
                    Status = b.Status,
                    TotalAmount = b.TotalAmount
                })
                .ToListAsync(cancellationToken);

            return bookings;
        }
    }
}
