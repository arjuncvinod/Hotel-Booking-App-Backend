using Hotel.Application.DTOs;
using Hotel.Application.Employee.Command.CreateEmployee;
using Hotel.Application.Employee.Command.DeleteEmployee;
using Hotel.Application.Employee.Command.UpdateEmployee;
using Hotel.Application.Employee.Queries.GetEmployeeById;
using Hotel.Application.Employee.Queries.GetEmployees;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController:ControllerBase
    {

        private readonly IMediator _mediator;

        public EmployeeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            GetEmployeesQuery query = new GetEmployeesQuery();
            var result = await _mediator.Send(query);
            return result is not null ? Ok(result) : NotFound();
        }

        [HttpGet("{id}")]

        public async Task<ActionResult> GetById(int id)
        {
            GetEmployeeByIdQuery query = new GetEmployeeByIdQuery();
            query.Id = id;
            return Ok(await _mediator.Send(query));
        }
   
        [HttpPost]
        public async Task<ActionResult> AddEmployee(CreateEmployeeCommand command)
        {

         EmployeeDto result = await _mediator.Send(command);

         return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id,UpdateEmployeeCommand command)
        {
            command.Id = id;

            var result = await _mediator.Send(command);
            return result is not null ? Ok(result) : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var command = new DeleteEmployeeCommand();
            command.Id = id;
            bool result = await _mediator.Send(command);
            return result ? Ok("User Deleted") : NotFound();
        }




    }
}
