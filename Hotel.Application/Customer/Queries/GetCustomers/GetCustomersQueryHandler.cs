using Hotel.Application.DTOs;
using Hotel.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Customer.Queries.GetCustomers
{
    public class GetCustomersQueryHandler:IRequestHandler<GetCustomersQuery, List<CustomerDto>>
    {

        private readonly HotelDbContext _context;

        public GetCustomersQueryHandler(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<List<CustomerDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            var customers = await _context.Customers.ToListAsync();

            List<CustomerDto> customerDtos = new List<CustomerDto>();

            foreach (var customer in customers) {

                CustomerDto customerDto = new CustomerDto
                {
                    Id = customer.Id,
                    FullName=customer.FullName,
                    PhoneNumber=customer.PhoneNumber,
                    Email=customer.Email,
                    IdproofNumber=customer.IdproofNumber,
                  
                };

                customerDtos.Add(customerDto);
            
            }

            return customerDtos;
        }
    }
}
