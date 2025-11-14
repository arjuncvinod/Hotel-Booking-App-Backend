using Hotel.Application.DTOs;
using Hotel.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Hotel.Commands.UpdateHotel
{
    public class UpdateHotelCommandHandler : IRequestHandler<UpdateHotelCommand, HotelDto>
    {
        private readonly HotelDbContext _context;

        public  UpdateHotelCommandHandler(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<HotelDto> Handle(UpdateHotelCommand request, CancellationToken cancellationToken)
        {
            var hotel= await _context.Hotels.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (hotel == null)
                throw new KeyNotFoundException($"Hotel with ID {request.Id} not found.");

            if (!string.IsNullOrWhiteSpace(request.Name))
                hotel.Name = request.Name;

            if (!string.IsNullOrWhiteSpace(request.Address))
                hotel.Address = request.Address;

            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
                hotel.PhoneNumber = request.PhoneNumber;

            if (!string.IsNullOrWhiteSpace(request.City))
                hotel.City = request.City;

            if (!string.IsNullOrWhiteSpace(request.Country))
                hotel.Country = request.Country;

            await _context.SaveChangesAsync(cancellationToken);

            return new HotelDto
            {
                Id= hotel.Id,
                Name = hotel.Name,
                Address = hotel.Address,
                PhoneNumber = hotel.PhoneNumber,
                City = hotel.City,
                Country = hotel.Country,
            };
        }
    }
}
