using Hotel.Application.DTOs;
using Hotel.Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Employee.Command.CreateEmployee
{
    public class CreateEmployeeCommandHandler:IRequestHandler<CreateEmployeeCommand,EmployeeDto>
    {
        private readonly HotelDbContext _context;

        public CreateEmployeeCommandHandler(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<EmployeeDto> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = new Domain.Entities.Employee
            {
                HotelId = request.HotelId,
                FullName = request.FullName,
                Role = request.Role,
                Email = request.Email
            };

           await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();

            return new EmployeeDto
            {
                Id=employee.Id,
                HotelId=employee.HotelId,
                FullName= employee.FullName,
                Email = employee.Email,
                Role = employee.Role

            };


        }
    }
}
