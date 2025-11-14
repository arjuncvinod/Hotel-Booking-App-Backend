using Hotel.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Room.Queries.GetRoomById
{
    public class GetRoomByIdQuery : IRequest<RoomDto>
    {
        public int Id { get; set; }
    }
}
