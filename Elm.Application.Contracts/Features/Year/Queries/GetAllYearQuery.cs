using Elm.Application.Contracts.Features.Year.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.Year.Queries
{
    public record GetAllYearQuery(int collegeId) : IRequest<Result<List<GetYearDto>>>;
}
