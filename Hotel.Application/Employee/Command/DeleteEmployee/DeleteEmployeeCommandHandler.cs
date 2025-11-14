using Hotel.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Employee.Command.DeleteEmployee
{
    public class DeleteEmployeeCommandHandler:IRequestHandler<DeleteEmployeeCommand,bool>
    {
        private readonly HotelDbContext _context;
        public DeleteEmployeeCommandHandler(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == request.Id);
            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with id {request.Id} not found");
            }
            _context.Employees.Remove(employee);
           await _context.SaveChangesAsync();
            return true;

        }
    }
}
