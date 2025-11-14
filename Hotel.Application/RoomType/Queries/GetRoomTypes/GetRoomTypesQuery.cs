using System;
using Hotel.Application.DTOs;
using MediatR;

namespace Hotel.Application.RoomType.Queries.GetRoomTypes;

public class GetRoomTypesQuery:IRequest<List<RoomTypeDto>>
{
}
