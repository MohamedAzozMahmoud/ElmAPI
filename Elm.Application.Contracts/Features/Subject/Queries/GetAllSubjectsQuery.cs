using Elm.Application.Contracts.Features.Subject.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.Subject.Queries
{
    public record GetAllSubjectsQuery(int pageSize, int pageNumber) : IRequest<Result<List<GetSubjectDto>>>;
}
