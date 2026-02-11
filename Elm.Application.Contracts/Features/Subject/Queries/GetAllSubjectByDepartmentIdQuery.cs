using Elm.Application.Contracts.Features.Subject.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.Subject.Queries
{
    public record GetAllSubjectByDepartmentIdQuery(int departmentId) : IRequest<Result<List<GetSubjectDto>>>;
}
