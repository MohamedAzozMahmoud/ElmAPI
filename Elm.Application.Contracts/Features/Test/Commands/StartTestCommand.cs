using Elm.Application.Contracts.Features.Test.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.Test.Commands
{
    public record StartTestCommand(int QuestionsBankId, int NumberOfQuestions)
        : IRequest<Result<TestDataDto>>;
}
