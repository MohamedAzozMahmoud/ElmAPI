using Elm.Application.Contracts.Features.Options.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.Options.Commands
{
    public record AddOptionCommand(string content, bool isCorrect, int questionId) : IRequest<Result<OptionsDto>>;
}
