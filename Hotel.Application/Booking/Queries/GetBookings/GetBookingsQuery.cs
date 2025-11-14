using Hotel.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Booking.Queries.GetBookings
{
    public class GetBookingsQuery : IRequest<List<BookingDto>>
    {
    }
}
