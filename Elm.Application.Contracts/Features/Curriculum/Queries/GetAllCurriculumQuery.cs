using Elm.Application.Contracts.Features.Curriculum.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.Curriculum.Queries
{
    public record GetAllCurriculumQuery(int departmentId, int yearId) : IRequest<Result<List<GetCurriculumDto>>>;
}
