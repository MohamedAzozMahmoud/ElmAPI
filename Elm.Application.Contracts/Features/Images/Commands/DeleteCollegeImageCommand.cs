using MediatR;

namespace Elm.Application.Contracts.Features.Images.Commands
{
    public record DeleteCollegeImageCommand(int collegeId, int id) : IRequest<Result<bool>>;
}
