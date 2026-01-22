using MediatR;

namespace Elm.Application.Contracts.Features.Subject.Commands
{
    public record DeleteSubjectCommand(
        int Id
    ) : IRequest<Result<bool>>;
}
