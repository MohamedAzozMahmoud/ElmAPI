using MediatR;

namespace Elm.Application.Contracts.Features.College.Commands
{
    public record DeleteCollegeCommand
        (int Id) : IRequest<Result<bool>>;
}
