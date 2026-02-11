using MediatR;

namespace Elm.Application.Contracts.Features.College.Commands
{
    public record UpdateCollegeCommand
        (
         int Id
        , string Name
        ) : IRequest<Result<bool>>;
}
