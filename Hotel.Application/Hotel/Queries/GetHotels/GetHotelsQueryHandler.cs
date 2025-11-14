using Hotel.Application.DTOs;
using Hotel.Application.Hotel.Queries.GetCustomers;
using Hotel.Application.Hotel.Queries.GetHotels;
using Hotel.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Hotel.Queries.GetHotels
{
    public class GetHotelsQueryHandler:IRequestHandler<GetHotelsQuery,List<HotelDto>>
    {
        private readonly HotelDbContext _context;

        public GetHotelsQueryHandler(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<List<HotelDto>> Handle(GetHotelsQuery request, CancellationToken cancellationToken)
        {
            var hotels = await _context.Hotels.ToListAsync(cancellationToken);

           var hotelDtos = new List<HotelDto>();

            foreach (var item in hotels)
            {
                var hotelDto = new HotelDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Address = item.Address,
                    City = item.City,
                    Country = item.Country,
                    PhoneNumber = item.PhoneNumber,
                };

                hotelDtos.Add(hotelDto);
            }


            return hotelDtos;
        }
    }
}
