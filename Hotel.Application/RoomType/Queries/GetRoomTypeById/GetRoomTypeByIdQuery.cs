using Hotel.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.RoomType.Queries.GetRoomTypeById
{
    public class GetRoomTypeByIdQuery:IRequest<RoomTypeDto>
    {
        public int Id { get; set; }
    }
}
