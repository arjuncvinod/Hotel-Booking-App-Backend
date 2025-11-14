using Hotel.Application.DTOs;
using Hotel.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Customer.Queries.GetCustomersById
{
    public class GetCustomerByIdQuery:IRequest<CustomerDto>
    {
        public int Id { get; set; }
    }
}
