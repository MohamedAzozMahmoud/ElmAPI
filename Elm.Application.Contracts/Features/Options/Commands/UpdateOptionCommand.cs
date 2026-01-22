using Elm.Application.Contracts.Features.Options.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.Options.Commands
{
    public record UpdateOptionCommand(int optionId, string content, bool isCorrect) : IRequest<Result<OptionsDto>>;
}
