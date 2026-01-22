using MediatR;

namespace Elm.Application.Contracts.Features.Options.Commands
{
    public record DeleteOptionCommand(int optionId) : IRequest<Result<bool>>;
}
