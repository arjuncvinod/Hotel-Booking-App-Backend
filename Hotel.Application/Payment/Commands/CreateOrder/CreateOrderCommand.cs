using MediatR;

namespace Hotel.Application.Payment.Commands.CreateOrder
{
    public record CreateOrderCommand(
        decimal Amount,
        string Receipt,
        int BookingId
    ) : IRequest<CreateOrderResponse>;

    public record CreateOrderResponse(
        string Id,
        int AmountInPaise,
        string Currency,
        string Receipt,
        string KeyId
    );
}