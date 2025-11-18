using Hotel.Domain.Entities;
using Hotel.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Hotel.Application.Auth
{
    public record RefreshCommand(string RefreshToken) : IRequest<AuthResult>;

    public class RefreshHandler : IRequestHandler<RefreshCommand, AuthResult>
    {
        private readonly HotelDbContext _db;
        private readonly IConfiguration _config;

        public RefreshHandler(HotelDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        public async Task<AuthResult> Handle(RefreshCommand cmd, CancellationToken ct)
        {
            var rt = await _db.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == cmd.RefreshToken && !t.Revoked && t.Expiry > DateTime.UtcNow, ct);

            if (rt == null) throw new SecurityTokenException("Invalid refresh token");

            rt.Revoked = true;

            var userInfo = rt.UserType == "Employee"
         ? await _db.Employees
             .Where(e => e.Id == rt.UserId)
             .Select(e => new { Email = e.Email, Role = "Admin" })
             .FirstAsync(ct)
         : await _db.Customers
             .Where(c => c.Id == rt.UserId)
             .Select(c => new { Email = c.Email, Role = "Customer" })
             .FirstAsync(ct);

            var newAccess = GenerateAccessToken(rt.UserId, userInfo.Email, userInfo.Role);
            var newRefresh = GenerateRefreshToken();

            _db.RefreshTokens.Add(new RefreshToken
            {
                Token = newRefresh,
                Expiry = DateTime.UtcNow.AddDays(7),
                UserType = rt.UserType,
                UserId = rt.UserId,
                CreatedAt = DateTime.UtcNow,
                ReplacedById = rt.Id
            });

            await _db.SaveChangesAsync(ct);
            return new AuthResult(newAccess, newRefresh);
        }

        private string GenerateAccessToken(int id, string email, string role)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim("role", role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
    }
}