using Hotel.Application.DTOs;
using Hotel.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.RoomType.Queries.GetRoomTypeById
{
    public class GetRoomByIdQueryHandler:IRequestHandler<GetRoomTypeByIdQuery, RoomTypeDto>
    {
        private readonly HotelDbContext _context;
        public GetRoomByIdQueryHandler(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<RoomTypeDto> Handle(GetRoomTypeByIdQuery request, CancellationToken cancellationToken)
        {
            Domain.Entities.RoomType? roomType = await _context.RoomTypes.FirstOrDefaultAsync(x=> x.Id == request.Id,cancellationToken);

            if (roomType == null) {

                throw new KeyNotFoundException($"Room type with id {request.Id} not found");
            }

            return new RoomTypeDto
            {
                Id = roomType.Id,
                TypeName = roomType.TypeName,
                Capacity = roomType.Capacity,
                Description = roomType.Description,

            };
        }

    }
}
