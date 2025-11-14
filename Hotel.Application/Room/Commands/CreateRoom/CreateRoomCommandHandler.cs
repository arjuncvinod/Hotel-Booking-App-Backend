using Hotel.Application.DTOs;
using Hotel.Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Room.Commands.CreateRoom
{
    public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, RoomDto>
    {
        private readonly HotelDbContext _context;

        public CreateRoomCommandHandler(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<RoomDto> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            var room = new Domain.Entities.Room
            {
                RoomNumber = request.RoomNumber,
                HotelId = request.HotelId,
                RoomTypeId = request.RoomTypeId,
                Status = request.Status,
                PricePerNight = request.PricePerNight
            };

            _context.Rooms.Add(room);
            await _context.SaveChangesAsync(cancellationToken);

            return new RoomDto
            {
                Id = room.Id,
                RoomNumber = room.RoomNumber,
                HotelId = room.HotelId,
                RoomTypeId = room.RoomTypeId,
                Status = room.Status,
                PricePerNight = room.PricePerNight
            };
        }
    }
}
