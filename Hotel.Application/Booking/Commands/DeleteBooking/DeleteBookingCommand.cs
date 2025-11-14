using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Booking.Commands.DeleteBooking
{
    public class DeleteBookingCommand : IRequest<int>
    {
        public int Id { get; set; }
    }
}
