using Elm.Application.Contracts.Features.Curriculum.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.Curriculum.Commands
{
    public record AddCurriculumCommand(
        int SubjectId,
        int YearId,
        int DepartmentId,
        int DoctorId) : IRequest<Result<CurriculumDto>>;
}
