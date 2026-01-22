using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.Files;
using Elm.Application.Contracts.Features.Files.Commands;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Files.Handlers
{
    public sealed class DeleteFileHandler : IRequestHandler<DeleteFileCommand, Result<bool>>
    {
        private readonly IGenericRepository<Domain.Entities.Files> filesRepository;
        private readonly IFileStorageService fileStorage;

        public DeleteFileHandler(IGenericRepository<Domain.Entities.Files> _filesRepository, IFileStorageService _fileStorage)
        {
            filesRepository = _filesRepository;
            fileStorage = _fileStorage;
        }
        public async Task<Result<bool>> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
        {
            var file = await filesRepository.GetByIdAsync(request.Id);
            if (file == null)
            {
                return Result<bool>.Failure("File not found.");
            }
            var deleteResult = await fileStorage.DeleteFile(file.StorageName, "Files");
            if (!deleteResult.IsSuccess)
            {
                return Result<bool>.Failure("Failed to delete file from storage.");
            }
            var result = await filesRepository.DeleteAsync(file.Id);
            if (result)
            {
                return Result<bool>.Failure("Failed to delete file.");
            }
            return Result<bool>.Success(true);
        }
    }
}
