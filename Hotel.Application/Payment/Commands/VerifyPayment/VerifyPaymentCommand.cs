using MediatR;

namespace Hotel.Application.Payment.Commands.VerifyPayment
{
    public record VerifyPaymentCommand(
        string RazorpayOrderId,
        string? RazorpayPaymentId = null,
        string? RazorpaySignature = null
    ) : IRequest<bool>;
}