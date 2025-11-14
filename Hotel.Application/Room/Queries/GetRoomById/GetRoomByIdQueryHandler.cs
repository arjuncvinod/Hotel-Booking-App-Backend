using Hotel.Application.DTOs;
using Hotel.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Room.Queries.GetRoomById
{
    public class GetRoomByIdQueryHandler : IRequestHandler<GetRoomByIdQuery, RoomDto>
    {
        private readonly HotelDbContext _context;

        public GetRoomByIdQueryHandler(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<RoomDto> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

            if (room == null)
            {
                throw new KeyNotFoundException($"Room with ID {request.Id} not found");
            }

            var roomDto = new RoomDto
            {
                Id = room.Id,
                RoomNumber = room.RoomNumber,
                HotelId = room.HotelId,
                RoomTypeId = room.RoomTypeId,
                Status = room.Status,
                PricePerNight = room.PricePerNight
            };

            return roomDto;
        }
    }
}
