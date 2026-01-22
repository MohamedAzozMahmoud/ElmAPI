using Elm.Application.Contracts.Features.Questions.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.Questions.Commands
{
    public record AddRingQuestionsCommand(int questionsBankId, List<AddQuestionsDto> QuestionsDtos) : IRequest<Result<bool>>;
}
