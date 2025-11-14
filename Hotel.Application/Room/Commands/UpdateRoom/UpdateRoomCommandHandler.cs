using Hotel.Application.DTOs;
using Hotel.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Application.Room.Commands.UpdateRoom
{
    public class UpdateRoomCommandHandler : IRequestHandler<UpdateRoomCommand, RoomDto>
    {
        private readonly HotelDbContext _context;

        public UpdateRoomCommandHandler(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<RoomDto> Handle(UpdateRoomCommand request, CancellationToken ct)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == request.Id, ct);

            if (room == null)
                throw new KeyNotFoundException($"Room with ID {request.Id} not found.");

            if (!string.IsNullOrWhiteSpace(request.RoomNumber))
                room.RoomNumber = request.RoomNumber;

            room.HotelId = request.HotelId;
            room.RoomTypeId = request.RoomTypeId;
            room.Status = request.Status;
            room.PricePerNight = request.PricePerNight;

            await _context.SaveChangesAsync(ct);

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
