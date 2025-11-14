using Hotel.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hotel.Application.Customer.Commands.DeleteCustomer
{
    public class DeleteCustomerCommand:IRequest<int>
    {
        public int Id { get; set; }
    }
}
