using MediatR;

namespace Elm.Application.Contracts.Features.Department.Commands
{
    public record PublishedDepartmentCommand(int Id) : IRequest<Result<bool>>;
}
