using Elm.Application.Contracts.Features.Curriculum.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.Curriculum.Queries
{
    public record GetCurriculumByStudentIdQuery(string UserId) : IRequest<Result<List<GetCurriculumDto>>>;
}
