using Elm.Application.Contracts.Features.Curriculum.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.Curriculum.Queries
{
    public record GetAllCurriculumForAdminQuery(int subjectId, int pageSize = 10, int pageNumber = 1) : IRequest<Result<List<AdminCurriculumDto>>>;
}
