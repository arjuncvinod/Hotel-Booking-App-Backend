using Hotel.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Review.Queries.GetReviewById
{
    public class GetReviewByIdQuery : IRequest<ReviewDto>
    {
        public int Id { get; set; }
    }
}
