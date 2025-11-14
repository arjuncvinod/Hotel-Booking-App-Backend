using Hotel.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Customer.Commands.DeleteCustomer
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, int>
    {
        private readonly HotelDbContext _context;

        public DeleteCustomerCommandHandler(HotelDbContext context) { 
        
            _context = context;
        }

        public async Task<int> Handle(DeleteCustomerCommand request, CancellationToken ct )
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (customer == null)
                throw new KeyNotFoundException($"Customer with Id {request.Id} not found");

               _context.Customers.Remove(customer);
               await  _context.SaveChangesAsync(ct);

            return 1;
            

        }
    }
}
