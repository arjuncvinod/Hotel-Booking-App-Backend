using Hotel.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Employee.Queries.GetEmployees
{
    public class GetEmployeesQuery:IRequest<List<EmployeeDto>>
    {
    }
}
