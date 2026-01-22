using MediatR;

namespace Elm.Application.Contracts.Features.Images.Commands
{
    public record DeleteUniversityImageCommand(int universityId, int id) : IRequest<Result<bool>>;
}
