using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.Files;
using Elm.Application.Contracts.Features.Files.Commands;
using MediatR;

namespace Elm.Application.Features.Files.Handlers
{
    public sealed class UploadFileHandler : IRequestHandler<UploadFileCommand, Result<string>>
    {
        private readonly IFileStorageService fileStorage;
        public UploadFileHandler(IFileStorageService fileStorage)
        {
            this.fileStorage = fileStorage;
        }
        public async Task<Result<string>> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            return await fileStorage.UploadFileAsync(request.curriculumId, request.uploadedById, request.FormFile, "Files");
        }
    }
}
