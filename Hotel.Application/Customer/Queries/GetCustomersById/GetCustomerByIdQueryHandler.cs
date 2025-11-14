using Hotel.Application.DTOs;
using Hotel.Domain.Entities;
using Hotel.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Customer.Queries.GetCustomersById
{
    public class GetCustomerByIdQueryHandler:IRequestHandler<GetCustomerByIdQuery,CustomerDto>
    {
        private readonly HotelDbContext _context;

        public GetCustomerByIdQueryHandler(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<CustomerDto> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {

            Domain.Entities.Customer customers = await _context.Customers.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (customers == null) {
            
                throw new KeyNotFoundException($"Customer with ID {request.Id} not found");

            }

            var customerDto = new CustomerDto
            {
                Id = customers.Id,
                FullName = customers.FullName,
                PhoneNumber = customers.PhoneNumber,
                Email = customers.Email,
                IdproofNumber = customers.IdproofNumber,
            };

            return customerDto;

        }
    }
}
