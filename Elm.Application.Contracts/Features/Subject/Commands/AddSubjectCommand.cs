using Elm.Application.Contracts.Features.Subject.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.Subject.Commands
{
    public record AddSubjectCommand(
        string Name,
        string Code
    ) : IRequest<Result<SubjectDto>>;
}
