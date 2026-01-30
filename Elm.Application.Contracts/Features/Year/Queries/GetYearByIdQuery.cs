using Elm.Application.Contracts.Features.Year.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.Year.Queries
{
    public record GetYearByIdQuery(int Id) : IRequest<Result<GetYearDto>>;
}
