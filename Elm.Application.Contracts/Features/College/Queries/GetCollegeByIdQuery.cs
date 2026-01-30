using Elm.Application.Contracts.Features.College.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.College.Queries
{
    public record GetCollegeByIdQuery
        (
        int Id
    ) : IRequest<Result<CollegeDto>>;
}
