using Hotel.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.RoomType.Command.DeleteRoomType
{
    public class DeleteRoomTypeCommandHandler : IRequestHandler<DeleteRoomTypeCommand, bool>
    {
        private readonly HotelDbContext _context;

        public DeleteRoomTypeCommandHandler(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteRoomTypeCommand request, CancellationToken ct)
        {
            var roomType = await _context.RoomTypes.FirstOrDefaultAsync(rt => rt.Id == request.Id, ct);

            if (roomType == null)
                throw new KeyNotFoundException($"RoomType with ID {request.Id} not found.");

            _context.RoomTypes.Remove(roomType);
            await _context.SaveChangesAsync(ct);

            return true;
        }
    }
}
