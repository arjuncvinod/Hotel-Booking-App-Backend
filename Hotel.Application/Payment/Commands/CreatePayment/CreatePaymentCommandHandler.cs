using Hotel.Application.DTOs;
using Hotel.Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Payment.Commands.CreatePayment
{
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, PaymentDto>
    {
        private readonly HotelDbContext _context;

        public CreatePaymentCommandHandler(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<PaymentDto> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = new Domain.Entities.Payment
            {
                BookingId = request.BookingId,
                PaymentDate = request.PaymentDate,
                Amount = request.Amount,
                Method = request.Method,
                Status = request.Status
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync(cancellationToken);

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
