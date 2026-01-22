using Elm.Application.Contracts.Features.Curriculum.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.Curriculum.Queries
{
    public record GetCurriculumByIdQuery(int Id) : IRequest<Result<CurriculumDto>>;
}
