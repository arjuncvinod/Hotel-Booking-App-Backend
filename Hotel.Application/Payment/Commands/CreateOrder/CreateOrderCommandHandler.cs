// Hotel.Application/Payment/Commands/CreateOrder/CreateOrderHandler.cs
using Hotel.Application.Payment.Commands.CreateOrder;
using Hotel.Domain.Entities;
using Hotel.Domain.Enums;
using Hotel.Infrastructure.Data;
using Hotel.Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Hotel.Application.Payment.Commands.CreateOrder
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, CreateOrderResponse>
    {
        private readonly RazorpayService _razorpay;
        private readonly HotelDbContext _db;
        private readonly string _keyId;

        public CreateOrderHandler(RazorpayService razorpay, HotelDbContext db, IConfiguration config)
        {
            _razorpay = razorpay;
            _db = db;
            _keyId = config["Razorpay:KeyId"]!;
        }

        public async Task<CreateOrderResponse> Handle(CreateOrderCommand cmd, CancellationToken ct)
        {
            var orderId = await _razorpay.CreateOrderAsync(cmd.Amount, cmd.Receipt);

            var payment = new Domain.Entities.Payment
            {
                BookingId = cmd.BookingId,
                Amount = cmd.Amount,
                PaymentDate = DateTime.UtcNow,
                Method = PaymentMethod.Online,
                Status = PaymentStatus.Pending,
                RazorpayOrderId = orderId
            };

            _db.Payments.Add(payment);
            await _db.SaveChangesAsync(ct);

            return new CreateOrderResponse(
                Id: orderId,
                AmountInPaise: (int)(cmd.Amount * 100),
                Currency: "INR",
                Receipt: cmd.Receipt,
                KeyId: _keyId
            );
        }
    }
}