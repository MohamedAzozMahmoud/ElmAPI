using Elm.Application.Contracts.Features.Department.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.Department.Queries
{
    public record GetAllDepartmentByCollegeIdQuery(int collegeId, int pageNumber, int pageSize): IRequest<Result<List<GetDepartmentDto>>>;
}
