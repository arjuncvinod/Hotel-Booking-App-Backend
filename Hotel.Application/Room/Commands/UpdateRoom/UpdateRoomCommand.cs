using Hotel.Application.DTOs;
using Hotel.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hotel.Application.Room.Commands.UpdateRoom
{
    public class UpdateRoomCommand : IRequest<RoomDto>
    {
        [JsonIgnore] public int Id { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public int HotelId { get; set; }
        public int RoomTypeId { get; set; }
        public RoomStatus Status { get; set; }
        public decimal PricePerNight { get; set; }
    }
}
