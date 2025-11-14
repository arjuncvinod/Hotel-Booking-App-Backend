using Hotel.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Booking.Queries.GetBookingById
{
    public class GetBookingByIdQuery : IRequest<BookingDto>
    {
        public int Id { get; set; }
    }
}
