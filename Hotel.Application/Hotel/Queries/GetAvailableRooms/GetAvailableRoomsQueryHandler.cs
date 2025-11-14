using Hotel.Application.DTOs;
using Hotel.Domain.Enums;
using Hotel.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Hotel.Application.Hotel.Queries.GetAvailableRooms
{
    public class GetAvailableRoomsQueryHandler
     : IRequestHandler<GetAvailableRoomsQuery, List<AvailableRoomsDto>>
    {
        private readonly HotelDbContext _context;

        public GetAvailableRoomsQueryHandler(HotelDbContext context) => _context = context;

        public async Task<List<AvailableRoomsDto>> Handle(GetAvailableRoomsQuery request, CancellationToken ct)
        {
            // === Validation ===
            if (request.CheckInDate >= request.CheckOutDate)
                throw new ArgumentException("CheckOutDate must be after CheckInDate.");
            if (string.IsNullOrWhiteSpace(request.Location))
                throw new ArgumentException("Location is required.");

            var location = request.Location.Trim();
            var checkIn = request.CheckInDate;
            var checkOut = request.CheckOutDate;

            // === Base query ===
            var query = _context.Hotels
                .Include(h => h.Rooms).ThenInclude(r => r.Bookings)
                .Include(h => h.Reviews)
                .AsQueryable();

            if (request.HotelId.HasValue)
                query = query.Where(h => h.Id == request.HotelId.Value);

            query = query.Where(h =>
                h.Rooms.Any(r => r.Status == RoomStatus.Available) &&
                (EF.Functions.Like(h.City, $"%{location}%") ||
                 EF.Functions.Like(h.Address, $"%{location}%") ||
                 EF.Functions.Like(h.Country, $"%{location}%")));

            // === available rooms ===
            var projected = query.Select(h => new
            {
                Hotel = h,
                AvailableRooms = h.Rooms
                    .Where(r => r.Status == RoomStatus.Available)
                    .Where(r => !r.Bookings.Any(b =>
                        (b.Status == BookingStatus.Confirmed || b.Status == BookingStatus.Pending) &&
                        b.CheckInDate < checkOut && b.CheckOutDate > checkIn))
                    .Select(r => new
                    {
                        r.Id,
                        r.RoomNumber,
                        r.HotelId,
                        r.RoomTypeId,
                        r.Status,
                        r.PricePerNight
                    })
                    .ToList()
            })
            .Where(x => x.AvailableRooms.Any());

            // === Final DTO  ===
            var result = projected.Select(x => new AvailableRoomsDto
            {
                Id = x.Hotel.Id,
                Name = x.Hotel.Name,
                Address = x.Hotel.Address,
                City = x.Hotel.City,
                Country = x.Hotel.Country,
                PhoneNumber = x.Hotel.PhoneNumber,

                AvailableRooms = x.AvailableRooms.Select(r => new RoomDto
                {
                    Id = r.Id,
                    RoomNumber = r.RoomNumber,
                    HotelId = r.HotelId,
                    RoomTypeId = r.RoomTypeId,
                    Status = r.Status,
                    PricePerNight = r.PricePerNight
                }).ToList(),

                AverageRating = x.Hotel.Reviews.Any() ? x.Hotel.Reviews.Average(rv => rv.Rating) : 0,
                LowestRoomPrice = x.AvailableRooms.Any() ? x.AvailableRooms.Min(r => r.PricePerNight) : 0
            });

            // === Apply MaxPrice Filter ===
            if (request.MaxPrice.HasValue)
            {
                var max = request.MaxPrice.Value;
                result = result.Where(dto => dto.LowestRoomPrice < max);
            }

            // === Sorting ===
            var sort = request.SortOption;

            if (sort == "pricing")
            {
                result = result
                    .OrderBy(dto => dto.LowestRoomPrice)
                    .ThenByDescending(dto => dto.AvailableRooms.Count)
                    .ThenByDescending(dto => dto.AverageRating)
                    .ThenBy(dto => dto.Name);
            }
            else if (sort == "rating")
            {
                result = result
                    .OrderByDescending(dto => dto.AverageRating)
                    .ThenByDescending(dto => dto.AvailableRooms.Count)
                    .ThenBy(dto => dto.LowestRoomPrice)
                    .ThenBy(dto => dto.Name);
            }
            else
            {
                result = result
                    .OrderByDescending(dto => dto.AvailableRooms.Count)
                    .ThenByDescending(dto => dto.AverageRating)
                    .ThenBy(dto => dto.LowestRoomPrice)
                    .ThenBy(dto => dto.Name);
            }

            return await result.ToListAsync(ct);
        }
    }
}