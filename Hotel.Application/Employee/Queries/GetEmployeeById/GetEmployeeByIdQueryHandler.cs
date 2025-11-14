using Hotel.Application.DTOs;
using Hotel.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Employee.Queries.GetEmployeeById
{
    public class GetEmployeeByIdQueryHandler:IRequestHandler<GetEmployeeByIdQuery,EmployeeDto>
    {
        private readonly HotelDbContext _context;   

       public GetEmployeeByIdQueryHandler( HotelDbContext context )
        {
            _context = context;
        }

        public async Task<EmployeeDto> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            Domain.Entities.Employee? employee = await _context.Employees.FirstOrDefaultAsync(cancellationToken);

            if (employee == null) {

                throw new KeyNotFoundException($"Employee with id {request.Id} not found");
            }

            return new EmployeeDto
            {
                Id = employee.Id,
                FullName = employee.FullName,
                Role = employee.Role,
                Email = employee.Email,
                HotelId = employee.Id,
            };
        }
    }
}
