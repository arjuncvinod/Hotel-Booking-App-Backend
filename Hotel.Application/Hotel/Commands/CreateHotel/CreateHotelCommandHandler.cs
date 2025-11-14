using Hotel.Application.DTOs;
using Hotel.Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Hotel.Commands.CreateHotel
{
    public class CreateHotelCommandHandler:IRequestHandler<CreateHotelCommand,HotelDto>
    {
        private readonly HotelDbContext _context;

        public CreateHotelCommandHandler(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<HotelDto> Handle(CreateHotelCommand request, CancellationToken cancellationToken)
        {
            var hotel = new Domain.Entities.Hotel
            {
                Name = request.Name,
                Address = request.Address,
                City = request.City,
                Country = request.Country,
                PhoneNumber = request.PhoneNumber,
            };

            _context.Hotels.Add(hotel);

            await _context.SaveChangesAsync(cancellationToken);

            return new HotelDto
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Address = hotel.Address,
                City = hotel.City,
                Country = hotel.Country,
                PhoneNumber = hotel.PhoneNumber
            };
        }
    }
}
