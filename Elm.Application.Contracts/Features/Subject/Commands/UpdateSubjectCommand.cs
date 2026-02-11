using MediatR;

namespace Elm.Application.Contracts.Features.Subject.Commands
{
    public record UpdateSubjectCommand(
        int Id,
        string Name,
        string Code
    ) : IRequest<Result<bool>>;
}
