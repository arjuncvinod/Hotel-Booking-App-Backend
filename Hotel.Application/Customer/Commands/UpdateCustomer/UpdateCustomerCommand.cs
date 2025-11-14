using Hotel.Application.DTOs;
using Hotel.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hotel.Application.Customer.Commands.UpdateCustomer
{
    public class UpdateCustomerCommand:IRequest<CustomerDto>
    { 
        [JsonIgnore] public int Id { get; set; } 
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string IdProofNumber { get; set; }
    }
}
