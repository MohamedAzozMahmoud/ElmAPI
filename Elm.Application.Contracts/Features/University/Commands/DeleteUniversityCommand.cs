using MediatR;

namespace Elm.Application.Contracts.Features.University.Commands
{
    public record DeleteUniversityCommand
        (
        int Id
    ) : IRequest<Result<bool>>;
}
