using Hotel.Application.DTOs;
using Hotel.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Application.Customer.Commands.UpdateCustomer;

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, CustomerDto>
{
    private readonly HotelDbContext _context;

    public UpdateCustomerCommandHandler(HotelDbContext context)
    {
        _context = context;
    }

    public async Task<CustomerDto> Handle(UpdateCustomerCommand request, CancellationToken ct)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == request.Id, ct);

        if (customer == null)
            throw new KeyNotFoundException($"Customer with ID {request.Id} not found.");

        if (!string.IsNullOrWhiteSpace(request.FullName))
            customer.FullName = request.FullName;

        if (!string.IsNullOrWhiteSpace(request.Email))
            customer.Email = request.Email;

        if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
            customer.PhoneNumber = request.PhoneNumber;

        if (!string.IsNullOrWhiteSpace(request.IdProofNumber))
            customer.IdproofNumber = request.IdProofNumber;

        await _context.SaveChangesAsync(ct);

        return new CustomerDto
        {

            FullName = customer.FullName,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            IdproofNumber = customer.IdproofNumber
            
        };
    }
}