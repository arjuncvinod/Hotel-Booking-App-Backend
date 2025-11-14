using Hotel.Application.DTOs;
using Hotel.Domain.Entities;
using Hotel.Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Customer.Commands.CreateCustomer
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, int>
    {
        private readonly HotelDbContext _context;

        public CreateCustomerCommandHandler(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            Domain.Entities.Customer customer = new Domain.Entities.Customer();

            customer.FullName = request.FullName;
            customer.PhoneNumber = request.PhoneNumber;
            customer.Email = request.Email;
            customer.IdproofNumber = request.IdproofNumber;

            _context.Customers.Add(customer);
           return await _context.SaveChangesAsync();

            
           
        }
    }
}
