using Hotel.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace Hotel.Application.Auth
{
    public record AuthResult(String AccessToken, String RefreshToken);

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
            var user = await _db.Employees
                .FirstOrDefaultAsync(x => x.Email == command.Email, ct);

            if (user == null || !BCrypt.Net.BCrypt.Verify(command.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid email or password");

            var accessToken = GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _db.SaveChangesAsync(ct);

            return new AuthResult(accessToken, refreshToken);
        }

        private string GenerateAccessToken(Domain.Entities.Employee user)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("role", user.Role)
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