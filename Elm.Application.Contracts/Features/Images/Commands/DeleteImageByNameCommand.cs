using MediatR;

namespace Elm.Application.Contracts.Features.Images.Commands
{
    public record DeleteImageByNameCommand(string fileName) : IRequest<Result<bool>>;
}
