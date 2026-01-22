using MediatR;

namespace Elm.Application.Contracts.Features.Curriculum.Commands
{
    public record DeleteCurriculumCommand(int Id) : IRequest<Result<bool>>;
}
