using Hotel.Application.DTOs;
using Hotel.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Application.Payment.Commands.UpdatePayment
{
    public class UpdatePaymentCommandHandler : IRequestHandler<UpdatePaymentCommand, PaymentDto>
    {
        private readonly HotelDbContext _context;

        public UpdatePaymentCommandHandler(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<PaymentDto> Handle(UpdatePaymentCommand request, CancellationToken ct)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.Id == request.Id, ct);

            if (payment == null)
                throw new KeyNotFoundException($"Payment with ID {request.Id} not found.");

            payment.BookingId = request.BookingId;
            payment.PaymentDate = request.PaymentDate;
            payment.Amount = request.Amount;
            payment.Method = request.Method;
            payment.Status = request.Status;

            await _context.SaveChangesAsync(ct);

            return new PaymentDto
            {
                Id = payment.Id,
                BookingId = payment.BookingId,
                PaymentDate = payment.PaymentDate,
                Amount = payment.Amount,
                Method = payment.Method,
                Status = payment.Status
            };
        }
    }
}
