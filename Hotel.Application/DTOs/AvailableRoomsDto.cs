using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.DTOs
{
    public class AvailableRoomsDto
    {

        public int Id {  get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
         public string Country { get; set; }

        public string PhoneNumber { get; set; }

        public int AvailableRoomsCount => AvailableRooms.Count;

        public decimal LowestRoomPrice { get; set; }
        public List<RoomDto> AvailableRooms { get; set; } = new();

        public double AverageRating { get; set; }






    }
}
