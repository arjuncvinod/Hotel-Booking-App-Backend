using Hotel.API.Middlewares;
using Hotel.Domain.Entities;
using Hotel.Infrastructure.Data;
using Hotel.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(x =>
    x.RegisterServicesFromAssembly(Assembly.Load("Hotel.Application")));



builder.Services.AddDbContext<HotelDbContext>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowTest", policy =>
    {
        policy.WithOrigins(
                "http://localhost:4200",
                "https://localhost:4200"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        var jwt = builder.Configuration.GetSection("Jwt");
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!))
        };
    });

builder.Services.AddScoped<RazorpayService>();


var app = builder.Build();



//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<HotelDbContext>();
//    db.Database.Migrate(); // Ensure latest migration

//    if (!db.Employees.Any(e => e.Email == "admin@gmail.com"))
//    {
//        var hash = BCrypt.Net.BCrypt.HashPassword("Admin123", 12);
//        db.Employees.Add(new Employee
//        {
//            HotelId = 1,
//            FullName = "Admin",
//            Role = "Admin",
//            Email = "admin@gmail.com",
//            PasswordHash = hash
//        });
//    }

//    if (!db.Customers.Any(c => c.Email == "arjun@gmail.com"))
//    {
//        var hash = BCrypt.Net.BCrypt.HashPassword("arjun123", 12);
//        db.Customers.Add(new Customer
//        {
//            FullName = "Arjun",
//            Email = "arjun@gmail.com",
//            PhoneNumber = "1234567890",
//            IdproofNumber = "ID123",
//            PasswordHash = hash
//        });
//    }
//    await db.SaveChangesAsync();
////}



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseCors("AllowTest");

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
