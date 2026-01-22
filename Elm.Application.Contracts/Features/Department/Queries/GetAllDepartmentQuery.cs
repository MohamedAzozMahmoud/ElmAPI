using Elm.Application.Contracts.Features.Department.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.Department.Queries
{
    public record GetAllDepartmentQuery(int yearId) : IRequest<Result<List<GetDepartmentDto>>>;
}
