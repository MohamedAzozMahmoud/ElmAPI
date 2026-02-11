using MediatR;

namespace Elm.Application.Contracts.Features.Year.Commands
{
    public record UpdateYearCommand(int Id, string Name) : IRequest<Result<bool>>;
}
