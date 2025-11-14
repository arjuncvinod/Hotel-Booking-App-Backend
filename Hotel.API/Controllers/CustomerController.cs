using Hotel.Application.Customer.Commands.CreateCustomer;
using Hotel.Application.Customer.Commands.DeleteCustomer;
using Hotel.Application.Customer.Commands.UpdateCustomer;
using Hotel.Application.Customer.Queries.GetCustomers;
using Hotel.Application.Customer.Queries.GetCustomersById;
using Hotel.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController:ControllerBase
    {

        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]

        public async Task<List<CustomerDto>> GetAll()
        {
            GetCustomersQuery query = new GetCustomersQuery();

            return await _mediator.Send(query);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<CustomerDto>> GetById(int id)
        {
                GetCustomerByIdQuery query = new GetCustomerByIdQuery();
                query.Id = id;

                CustomerDto cus = await _mediator.Send(query);
                return Ok(cus);
        }

        [HttpPost]
        public async Task<ActionResult> AddCustomer(CreateCustomerCommand command)

        {
           await _mediator.Send(command);

            return StatusCode(201,"Customer created");
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerDto>> UpdateCustomer(int id, [FromBody] UpdateCustomerCommand command)
        { 

                command.Id = id;
                var result = await _mediator.Send(command);
                return Ok(result); 
            
        }


        [HttpDelete("{id}")]

        public async Task<ActionResult> Delete(int id)
        {
            DeleteCustomerCommand command=new DeleteCustomerCommand();
           
                command.Id = id;
                var result = await _mediator.Send(command);
     
            return Ok("User Deleted");
        }

    }
}
