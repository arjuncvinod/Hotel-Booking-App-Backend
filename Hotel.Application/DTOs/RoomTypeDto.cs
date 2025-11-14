using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.DTOs;

public class RoomTypeDto
{
    public int Id { get; set; }
    public string TypeName { get; set; }
    public string Description { get; set; }
    public int Capacity { get; set; }
}
