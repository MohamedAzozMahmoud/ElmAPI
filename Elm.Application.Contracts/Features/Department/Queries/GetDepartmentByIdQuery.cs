using Elm.Application.Contracts.Features.Department.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.Department.Queries
{
    public record GetDepartmentByIdQuery(int Id) : IRequest<Result<GetDepartmentDto>>;
}
