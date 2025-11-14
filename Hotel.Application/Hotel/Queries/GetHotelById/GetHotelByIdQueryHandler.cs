using Hotel.Application.DTOs;
using Hotel.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Hotel.Queries.GetHotelById
{
    public class GetHotelByIdQueryHandler:IRequestHandler<GetHotelByIdQuery, HotelDto>
    {
        private readonly HotelDbContext _context;

        public GetHotelByIdQueryHandler(HotelDbContext context )
        {
            _context = context;
        }

        public async Task<HotelDto> Handle(GetHotelByIdQuery request, CancellationToken cancellationToken)
        {
           var hotel = await _context.Hotels.FirstOrDefaultAsync(x=>x.Id== request.Id);

            if (hotel == null)
            {

                throw new KeyNotFoundException($"customer with id {request.Id} not found");
            }

            return new HotelDto
            {
                Id = hotel.Id,
                Name=hotel.Name,
                Address=hotel.Address,
                City=hotel.City,
                Country=hotel.Country,
                PhoneNumber=hotel.PhoneNumber,
            };
        }
    }
}
