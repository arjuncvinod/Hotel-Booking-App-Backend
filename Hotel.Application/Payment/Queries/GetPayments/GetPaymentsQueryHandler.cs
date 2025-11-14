using Hotel.Application.DTOs;
using Hotel.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Payment.Queries.GetPayments
{
    public class GetPaymentsQueryHandler : IRequestHandler<GetPaymentsQuery, List<PaymentDto>>
    {
        private readonly HotelDbContext _context;

        public GetPaymentsQueryHandler(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<List<PaymentDto>> Handle(GetPaymentsQuery request, CancellationToken cancellationToken)
        {
            var payments = await _context.Payments.ToListAsync(cancellationToken);

            var paymentDtos = new List<PaymentDto>();

            foreach (var payment in payments)
            {
                var paymentDto = new PaymentDto
                {
                    Id = payment.Id,
                    BookingId = payment.BookingId,
                    PaymentDate = payment.PaymentDate,
                    Amount = payment.Amount,
                    Method = payment.Method,
                    Status = payment.Status
                };

                paymentDtos.Add(paymentDto);
            }

            return paymentDtos;
        }
    }
}
