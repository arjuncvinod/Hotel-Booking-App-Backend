using System;
using Hotel.Application.DTOs;
using Hotel.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Hotel.Application.RoomType.Queries.GetRoomTypes;

public class GetRoomTypesQueryHandler:IRequestHandler<GetRoomTypesQuery,List<RoomTypeDto>>
{
    private readonly HotelDbContext _context;
    public GetRoomTypesQueryHandler(HotelDbContext context)
    {
        _context = context;
    }

    public async Task<List<RoomTypeDto>> Handle(GetRoomTypesQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.RoomTypes.ToListAsync(cancellationToken);

        List<RoomTypeDto> roomTypeDtos = result.Select(rt => new RoomTypeDto
        {
            Id = rt.Id,
            TypeName = rt.TypeName,
            Description = rt.Description,
            Capacity = rt.Capacity,
        }).ToList();

        return roomTypeDtos;
    }
}