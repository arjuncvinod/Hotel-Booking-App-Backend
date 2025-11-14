using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.Entities
{
    public class Customer
    {

        public int Id { get; set; }
        
        public string FullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string IdproofNumber { get; set; }

        public List<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
