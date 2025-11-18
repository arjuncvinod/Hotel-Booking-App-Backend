using Hotel.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Application.Auth
{
    public record RevokeCommand(int UserId, string UserType) : IRequest;

    public class RevokeHandler : IRequestHandler<RevokeCommand>
    {
        private readonly HotelDbContext _db;
        public RevokeHandler(HotelDbContext db) => _db = db;

        public async Task Handle(RevokeCommand cmd, CancellationToken ct)
        {
            var tokens = await _db.RefreshTokens
                .Where(t => t.UserId == cmd.UserId && t.UserType == cmd.UserType && !t.Revoked)
                .ToListAsync(ct);

            foreach (var t in tokens) t.Revoked = true;
            await _db.SaveChangesAsync(ct);
        }
    }
}