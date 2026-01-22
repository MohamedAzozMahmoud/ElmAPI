using MediatR;

namespace Elm.Application.Contracts.Features.Department.Commands
{
    public record DeleteDepartmentCommand(int Id) : IRequest<Result<bool>>;
}
