using Hotel.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Payment.Commands.DeletePayment
{
    public class DeletePaymentCommandHandler : IRequestHandler<DeletePaymentCommand, int>
    {
        private readonly HotelDbContext _context;

        public DeletePaymentCommandHandler(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(DeletePaymentCommand request, CancellationToken ct)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.Id == request.Id, ct);

            if (payment == null)
                throw new KeyNotFoundException($"Payment with ID {request.Id} not found.");

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync(ct);

            return 1;
        }
    }
}
