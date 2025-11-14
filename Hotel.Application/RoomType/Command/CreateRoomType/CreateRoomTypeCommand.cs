using Hotel.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.RoomType.Command.CreateRoomType
{
    public class CreateRoomTypeCommand:IRequest<RoomTypeDto>
    {
        public string TypeName { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
    }

}
