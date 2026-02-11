using MediatR;

namespace Elm.Application.Contracts.Features.Curriculum.Commands
{
    public record PublishedCurriculumCommand(int Id) : IRequest<Result<bool>>;
}
