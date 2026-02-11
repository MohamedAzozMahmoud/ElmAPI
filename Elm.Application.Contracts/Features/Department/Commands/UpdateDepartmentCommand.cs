using Elm.Domain.Enums;
using MediatR;

namespace Elm.Application.Contracts.Features.Department.Commands
{
    public record UpdateDepartmentCommand(int Id, string Name, bool IsPaid, TypeOfDepartment Type) : IRequest<Result<bool>>;
}
