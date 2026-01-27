using Elm.Application.Contracts.Features.University.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.University.Queries
{
    public record GetUniversityByNameQuery
    (
        string Name
    ) : IRequest<Result<UniversityDetialsDto>>;
}
