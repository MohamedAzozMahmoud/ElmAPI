using Elm.Application.Contracts.Features.Files.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.Files.Queries
{
    public record DownloadFileCommand(string fileName) : IRequest<Result<FileResponse>>;
}
