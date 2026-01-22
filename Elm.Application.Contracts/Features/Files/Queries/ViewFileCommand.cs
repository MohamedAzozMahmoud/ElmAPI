using Elm.Application.Contracts.Features.Images.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.Files.Queries
{
    public record ViewFileCommand(string fileName) : IRequest<Result<ImageDto>>;
}
