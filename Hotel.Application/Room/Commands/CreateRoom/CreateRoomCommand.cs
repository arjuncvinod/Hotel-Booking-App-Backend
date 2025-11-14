using Hotel.Application.DTOs;
using Hotel.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Room.Commands.CreateRoom
{
    public class CreateRoomCommand : IRequest<RoomDto>
    {
        public string RoomNumber { get; set; } = string.Empty;
        public int HotelId { get; set; }
        public int RoomTypeId { get; set; }
        public RoomStatus Status { get; set; }
        public decimal PricePerNight { get; set; }
    }
}
