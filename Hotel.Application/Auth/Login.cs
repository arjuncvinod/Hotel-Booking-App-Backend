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
    public record AuthResult(string AccessToken, string RefreshToken);
    public record LoginCommand(string Email, string Password) : IRequest<AuthResult>;

    public class LoginHandler : IRequestHandler<LoginCommand, AuthResult>
    {
        private readonly HotelDbContext _db;
        private readonly IConfiguration _config;

        public LoginHandler(HotelDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        public async Task<AuthResult> Handle(LoginCommand command, CancellationToken ct)
        {
            var user = await _db.Employees.FirstOrDefaultAsync(x => x.Email == command.Email, ct);
            string role = "Admin";
            int userId = 0;
            string email = command.Email;

            if (user != null && BCrypt.Net.BCrypt.Verify(command.Password, user.PasswordHash))
            {
                userId = user.Id;
            }
            else
            {
                var customer = await _db.Customers.FirstOrDefaultAsync(x => x.Email == command.Email, ct);
                if (customer != null && BCrypt.Net.BCrypt.Verify(command.Password, customer.PasswordHash))
                {
                    userId = customer.Id;
                    role = "Customer";
                }
                else
                {
                    throw new UnauthorizedAccessException("Invalid email or password");
                }
            }

            var accessToken = GenerateAccessToken(userId, email, role);
            var refreshToken = GenerateRefreshToken();

            var tokenEntity = new RefreshToken
            {
                Token = refreshToken,
                Expiry = DateTime.UtcNow.AddDays(7),
                UserType = role == "Admin" ? "Employee" : "Customer",
                UserId = userId
            };
            _db.RefreshTokens.Add(tokenEntity);
            await _db.SaveChangesAsync(ct);

            return new AuthResult(accessToken, refreshToken);
        }

        private string GenerateAccessToken(int userId, string email, string role)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
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
            var random = new byte[64];
            RandomNumberGenerator.Fill(random);
            return Convert.ToBase64String(random);
        }
    }
}