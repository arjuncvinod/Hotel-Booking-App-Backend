using Hotel.Application.DTOs;
using Hotel.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Employee.Command.UpdateEmployee
{
    public class UpdateEmployeeCommandHandler:IRequestHandler<UpdateEmployeeCommand,EmployeeDto>
    {
        private readonly HotelDbContext _context;

        public UpdateEmployeeCommandHandler(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<EmployeeDto> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (employee == null)
                throw new KeyNotFoundException($"Employee with ID {request.Id} not found.");

            if (!string.IsNullOrWhiteSpace(request.FullName))
                employee.FullName = request.FullName;

            if (request.HotelId > 0)
                employee.HotelId = request.HotelId;

            if (!string.IsNullOrWhiteSpace(request.Email))
                employee.Email = request.Email;

            if (!string.IsNullOrWhiteSpace(request.Role))
                employee.Role = request.Role;


            await _context.SaveChangesAsync(cancellationToken);

            return new EmployeeDto
            {
                Id = employee.Id,
                HotelId = employee.HotelId,
                FullName = employee.FullName,
                Email = employee.Email,
                Role = employee.Role,

            };
        }
    }
}
