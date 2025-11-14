using Hotel.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Room.Commands.DeleteRoom
{
    public class DeleteRoomCommandHandler : IRequestHandler<DeleteRoomCommand, int>
    {
        private readonly HotelDbContext _context;

        public DeleteRoomCommandHandler(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(DeleteRoomCommand request, CancellationToken ct)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == request.Id, ct);

            if (room == null)
                throw new KeyNotFoundException($"Room with ID {request.Id} not found.");

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync(ct);

            return 1;
        }
    }
}
