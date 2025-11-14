using Hotel.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Hotel.DeleteHotel
{
    public class DeleteHotelCommandHandler:IRequestHandler<DeleteHotelCommand,bool>
    {

        private readonly HotelDbContext _context;

        public DeleteHotelCommandHandler(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteHotelCommand request, CancellationToken cancellationToken)
        {
            var hotel = await _context.Hotels.FirstOrDefaultAsync(x=>x.Id == request.Id);

            if (hotel == null) {
                throw new KeyNotFoundException($"Hotel with Id {request.Id} not found");
            }

                _context.Hotels.Remove(hotel);
           await _context.SaveChangesAsync();

            return true;
        }
    }
}
