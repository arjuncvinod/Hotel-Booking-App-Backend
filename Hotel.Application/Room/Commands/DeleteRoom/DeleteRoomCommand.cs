using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Room.Commands.DeleteRoom
{
    public class DeleteRoomCommand : IRequest<int>
    {
        public int Id { get; set; }
    }
}
