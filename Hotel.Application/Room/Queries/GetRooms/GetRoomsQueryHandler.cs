using Hotel.Application.DTOs;
using Hotel.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Room.Queries.GetRooms
{
    public class GetRoomsQueryHandler : IRequestHandler<GetRoomsQuery, List<RoomDto>>
    {
        private readonly HotelDbContext _context;

        public GetRoomsQueryHandler(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<List<RoomDto>> Handle(GetRoomsQuery request, CancellationToken cancellationToken)
        {
            var rooms = await _context.Rooms.ToListAsync(cancellationToken);

            var roomDtos = new List<RoomDto>();

            foreach (var room in rooms)
            {
                var roomDto = new RoomDto
                {
                    Id = room.Id,
                    RoomNumber = room.RoomNumber,
                    HotelId = room.HotelId,
                    RoomTypeId = room.RoomTypeId,
                    Status = room.Status,
                    PricePerNight = room.PricePerNight
                };

                roomDtos.Add(roomDto);
            }

            return roomDtos;
        }
    }
}
