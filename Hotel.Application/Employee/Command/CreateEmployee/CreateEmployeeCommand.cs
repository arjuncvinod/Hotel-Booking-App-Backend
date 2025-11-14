using Hotel.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Employee.Command.CreateEmployee
{
    public class CreateEmployeeCommand:IRequest<EmployeeDto>
    {
        public int HotelId { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
    }
}
