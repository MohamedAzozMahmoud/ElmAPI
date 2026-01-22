using Elm.Application.Contracts.Features.Images.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.Images.Queries
{
    public record showImageFromUrlCommand(string FileName) : IRequest<Result<ImageDto>>;
}
