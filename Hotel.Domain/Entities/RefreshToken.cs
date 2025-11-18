using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = null!;
        public DateTime Expiry { get; set; }
        public bool Revoked { get; set; } = false;
        public string UserType { get; set; } = null!; 
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? ReplacedById { get; set; }
    }
}
