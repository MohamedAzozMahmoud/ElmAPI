using MediatR;

namespace Elm.Application.Contracts.Features.Files.Commands
{
    public record DeleteFileCommand(int Id) : IRequest<Result<bool>>;
}
