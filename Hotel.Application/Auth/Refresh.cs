
using Hotel.Application.Auth;
using Hotel.Domain.Entities;
using Hotel.Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Application.Auth;

public record RefreshCommand(string AccessToken, string RefreshToken) : IRequest<AuthResult>;

public class RefreshHandler : IRequestHandler<RefreshCommand, AuthResult>
{
    private readonly HotelDbContext _db;
    private readonly IConfiguration _config;

    public RefreshHandler(HotelDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    public async Task<AuthResult> Handle(RefreshCommand command, CancellationToken ct)
    {
        var principal = GetPrincipalFromExpiredToken(command.AccessToken);
        var userId = int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var user = await _db.Employees.FindAsync(new object[] { userId }, ct);
        if (user == null ||
            user.RefreshToken != command.RefreshToken ||
            user.RefreshTokenExpiry <= DateTime.UtcNow)
            throw new SecurityTokenException("Invalid refresh token");

        var newAccessToken = GenerateAccessToken(user);
        var newRefreshToken = GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await _db.SaveChangesAsync(ct);

        return new AuthResult(newAccessToken, newRefreshToken);
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var validation = new TokenValidationParameters
        {
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateIssuer = false,
            ValidateAudience = false
        };

        var handler = new JwtSecurityTokenHandler();
        var principal = handler.ValidateToken(token, validation, out _);
        return principal;
    }

    private string GenerateAccessToken(Employee user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var jwt = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    private string GenerateRefreshToken()
    {
        var random = new byte[64];
        RandomNumberGenerator.Fill(random);
        return Convert.ToBase64String(random);
    }
}