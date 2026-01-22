using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.Files;
using Elm.Application.Contracts.Features.Files.Queries;
using Elm.Application.Contracts.Features.Images.DTOs;
using MediatR;

namespace Elm.Application.Features.Files.Handlers
{
    public sealed class ViewFileHandler : IRequestHandler<ViewFileCommand, Result<ImageDto>>
    {
        private readonly IFileStorageService fileStorage;
        public ViewFileHandler(IFileStorageService _fileStorage)
        {
            fileStorage = _fileStorage;
        }
        public async Task<Result<ImageDto>> Handle(ViewFileCommand request, CancellationToken cancellationToken)
        {
            return await fileStorage.GetFileAsync(request.fileName, "Files");
        }
    }
}
