using Hotel.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hotel.Application.Employee.Command.UpdateEmployee
{
    public class UpdateEmployeeCommand:IRequest<EmployeeDto>
    {
        [JsonIgnore]public int Id { get; set; }
        public int HotelId { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
    }
}
