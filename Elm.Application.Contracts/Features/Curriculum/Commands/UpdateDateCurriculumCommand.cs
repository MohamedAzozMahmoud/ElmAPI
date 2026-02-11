using MediatR;

namespace Elm.Application.Contracts.Features.Curriculum.Commands
{
    public record UpdateDateCurriculumCommand(int Id, byte StartMonth, byte EndMonth) : IRequest<Result<bool>>;
}
