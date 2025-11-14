using Hotel.Application.DTOs;
using Hotel.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Hotel.Queries.GetAvailableRooms
{
    public class GetAvailableRoomsQuery:IRequest<List<AvailableRoomsDto>>
    {
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }

        public String Location { get; set; }

        public int? HotelId { get; set; }

        public string? SortOption { get; set; }

        public decimal? MaxPrice { get; set; }
    }
}
