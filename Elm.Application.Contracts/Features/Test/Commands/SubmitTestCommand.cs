using Elm.Application.Contracts.Features.Test.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.Test.Commands
{
    public record SubmitTestCommand
    (
        Guid TestSessionId,
        List<SubmittedAnswerDto> Answers
    ) : IRequest<Result<TestResultDto>>;
}
