using Hotel.Application.DTOs;
using Hotel.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Payment.Queries.GetPaymentById
{
    public class GetPaymentByIdQueryHandler : IRequestHandler<GetPaymentByIdQuery, PaymentDto>
    {
        private readonly HotelDbContext _context;

        public GetPaymentByIdQueryHandler(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<PaymentDto> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (payment == null)
            {
                throw new KeyNotFoundException($"Payment with ID {request.Id} not found");
            }

            var paymentDto = new PaymentDto
            {
                Id = payment.Id,
                BookingId = payment.BookingId,
                PaymentDate = payment.PaymentDate,
                Amount = payment.Amount,
                Method = payment.Method,
                Status = payment.Status
            };

            return paymentDto;
        }
    }
}
