using Hotel.Application.DTOs;
using Hotel.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Employee.Queries.GetEmployees
{
    public class GetEmployeesQueryHandler:IRequestHandler<GetEmployeesQuery,List<EmployeeDto>>
    {
        private readonly HotelDbContext _context;

        public GetEmployeesQueryHandler(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<List<EmployeeDto>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
        {
            List<Domain.Entities.Employee> employees = await _context.Employees.ToListAsync(cancellationToken);

            var employeesDto  = new List<EmployeeDto>();

            foreach(var employee in employees)
            {
                var employeeDto = new EmployeeDto
                {
                    Id = employee.Id,
                    HotelId = employee.Id,
                    FullName = employee.FullName,
                    Role   = employee.Role,
                    Email = employee.Email,  
                };

                employeesDto.Add(employeeDto);
                
            }

            return employeesDto;
           


        }
    }
}
