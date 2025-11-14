using Hotel.Application.DTOs;
using Hotel.Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.RoomType.Command.CreateRoomType
{
    public class CreateRoomTypeCommandHandler:IRequestHandler<CreateRoomTypeCommand,RoomTypeDto>
    {
        private readonly HotelDbContext _context;

        public CreateRoomTypeCommandHandler(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<RoomTypeDto> Handle(CreateRoomTypeCommand request, CancellationToken cancellationToken)
        {
            var roomType = new Domain.Entities.RoomType
            {
                TypeName = request.TypeName,
                Description = request.Description,
                Capacity = request.Capacity,
            };
            await _context.RoomTypes.AddAsync(roomType);
            await _context.SaveChangesAsync(cancellationToken);
        
            return new RoomTypeDto
            {
                Id = roomType.Id,
                TypeName = roomType.TypeName,
                Description = roomType.Description,
                Capacity = roomType.Capacity,
            };
        }

      
    }
}
