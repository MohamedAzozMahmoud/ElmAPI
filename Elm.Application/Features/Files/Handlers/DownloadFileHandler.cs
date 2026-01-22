using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.Files;
using Elm.Application.Contracts.Features.Files.DTOs;
using Elm.Application.Contracts.Features.Files.Queries;
using MediatR;

namespace Elm.Application.Features.Files.Handlers
{
    public sealed class DownloadFileHandler : IRequestHandler<DownloadFileCommand, Result<FileResponse>>
    {
        private readonly IFileStorageService fileStorage;
        public DownloadFileHandler(IFileStorageService _fileStorage)
        {
            fileStorage = _fileStorage;
        }
        public async Task<Result<FileResponse>> Handle(DownloadFileCommand request, CancellationToken cancellationToken)
        {
            return await fileStorage.DownloadFileAsync(request.fileName, "Files");
        }
    }
}
