using MediatR;

namespace Elm.Application.Contracts.Features.Year.Commands
{
    public record DeleteYearCommand(int Id) : IRequest<Result<bool>>;
}
