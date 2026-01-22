using Elm.Application.Contracts.Features.Department.DTOs;
using Elm.Domain.Enums;
using MediatR;

namespace Elm.Application.Contracts.Features.Department.Commands
{
    public record AddDepartmentCommand(string Name, bool IsPaid, TypeOfDepartment Type, int collegeId) : IRequest<Result<DepartmentDto>>;
}
