using Hotel.Domain.Enums;
using Hotel.Infrastructure.Data;
using Hotel.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Application.Payment.Commands.VerifyPayment
{
    public class VerifyPaymentHandler : IRequestHandler<VerifyPaymentCommand, bool>
    {
        private readonly RazorpayService _razorpay;
        private readonly HotelDbContext _db;

        public VerifyPaymentHandler(RazorpayService razorpay, HotelDbContext db)
        {
            _razorpay = razorpay;
            _db = db;
        }

        public async Task<bool> Handle(VerifyPaymentCommand cmd, CancellationToken ct)
        {
            bool isValid = _razorpay.VerifySignature(
                cmd.RazorpayOrderId,
                cmd.RazorpayPaymentId,
                cmd.RazorpaySignature
            );

            var payment = await _db.Payments
                .Include(p => p.Booking)
                .FirstOrDefaultAsync(p => p.RazorpayOrderId == cmd.RazorpayOrderId, ct);

            if (payment == null) return false;

            if (isValid && cmd.RazorpayPaymentId != null)
            {
                payment.Status = PaymentStatus.Paid;
                payment.RazorpayPaymentId = cmd.RazorpayPaymentId;
                payment.RazorpaySignature = cmd.RazorpaySignature;

                if (payment.Booking != null)
                    payment.Booking.Status = BookingStatus.Confirmed;
            }
            else
            {
                payment.Status = PaymentStatus.Failed;
                if (payment.Booking != null)
                    payment.Booking.Status = BookingStatus.Cancelled;
            }

            await _db.SaveChangesAsync(ct);
            return isValid;
        }
    }
}