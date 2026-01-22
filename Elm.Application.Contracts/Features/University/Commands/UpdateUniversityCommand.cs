using Elm.Application.Contracts.Features.University.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.University.Commands
{
    public record UpdateUniversityCommand
        (
        int Id,
        string Name
    ) : IRequest<Result<UniversityDto>>;
}
