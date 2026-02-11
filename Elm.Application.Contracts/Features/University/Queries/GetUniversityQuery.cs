using Elm.Application.Contracts.Features.University.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.University.Queries
{
    public record GetUniversityQuery() : IRequest<Result<UniversityDetialsDto>>;
}
