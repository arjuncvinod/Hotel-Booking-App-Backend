using Hotel.Application.DTOs;
using Hotel.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Application.Hotel.Queries.GetHotelById
{
    public class GetHotelByIdQueryHandler : IRequestHandler<GetHotelByIdQuery, HotelDto>
    {
        private readonly HotelDbContext _context;

        public GetHotelByIdQueryHandler(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<HotelDto> Handle(GetHotelByIdQuery request, CancellationToken cancellationToken)
        {
            var hotel = await _context.Hotels
                .Include(h => h.Rooms)             
                .ThenInclude(r => r.RoomType)           
                .AsSplitQuery()                          
                .FirstOrDefaultAsync(h => h.Id == request.Id, cancellationToken);

            if (hotel == null)
            {
                throw new KeyNotFoundException($"Hotel with id {request.Id} not found");
            }

            return new HotelDto
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Address = hotel.Address,
                City = hotel.City,
                Country = hotel.Country,
                PhoneNumber = hotel.PhoneNumber,
                Rooms = hotel.Rooms.Select(r => new RoomDto
                {
                    Id = r.Id,
                    RoomNumber = r.RoomNumber,
                    HotelId = r.HotelId,
                    RoomTypeId = r.RoomTypeId,
                    Status = r.Status,
                    PricePerNight = r.PricePerNight
                }).ToList()
            };
        }
    }
}