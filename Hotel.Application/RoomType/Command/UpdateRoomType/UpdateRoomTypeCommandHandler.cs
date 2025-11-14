using Hotel.Application.DTOs;
using Hotel.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Application.RoomType.Command.UpdateRoomType
{
    public class UpdateRoomTypeCommandHandler : IRequestHandler<UpdateRoomTypeCommand, RoomTypeDto>
    {
        private readonly HotelDbContext _context;

        public UpdateRoomTypeCommandHandler(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<RoomTypeDto> Handle(UpdateRoomTypeCommand request, CancellationToken ct)
        {
            var roomType = await _context.RoomTypes.FirstOrDefaultAsync(rt => rt.Id == request.Id, ct);

            if (roomType == null)
                throw new KeyNotFoundException($"RoomType with ID {request.Id} not found.");

            if (!string.IsNullOrWhiteSpace(request.TypeName))
                roomType.TypeName = request.TypeName;

            if (!string.IsNullOrWhiteSpace(request.Description))
                roomType.Description = request.Description;

            roomType.Capacity = request.Capacity;

            await _context.SaveChangesAsync(ct);

            return new RoomTypeDto
            {
                Id = roomType.Id,
                TypeName = roomType.TypeName,
                Description = roomType.Description,
                Capacity = roomType.Capacity
            };
        }
    }
}
