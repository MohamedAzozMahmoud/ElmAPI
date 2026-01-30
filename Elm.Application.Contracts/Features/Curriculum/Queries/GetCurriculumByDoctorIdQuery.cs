using Elm.Application.Contracts.Features.Curriculum.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.Curriculum.Queries
{
    public record GetCurriculumByDoctorIdQuery(int doctorId) : IRequest<Result<List<GetCurriculumDto>>>;
}
