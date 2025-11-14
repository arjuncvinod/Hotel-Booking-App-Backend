using Hotel.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Application.Auth;

public record RevokeRequest(string RefreshToken);

public record RevokeCommand(string RefreshToken) : IRequest;

public class RevokeHandler : IRequestHandler<RevokeCommand>
{
    private readonly HotelDbContext _db;

    public RevokeHandler(HotelDbContext db) => _db = db;

    public async Task Handle(RevokeCommand command, CancellationToken ct)
    {
        var user = await _db.Employees
            .FirstOrDefaultAsync(e => e.RefreshToken == command.RefreshToken, ct);

        if (user != null)
        {
            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;
            await _db.SaveChangesAsync(ct);
        }
    }
}