using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.Files;
using Elm.Application.Contracts.Features.College.Commands;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.College.Handlers
{
    public sealed class DeleteCollegeHandler : IRequestHandler<DeleteCollegeCommand, Result<bool>>
    {
        private readonly ICollegeRepository repository;
        private readonly IFileStorageService fileStorage;

        public DeleteCollegeHandler(ICollegeRepository _repository, IFileStorageService _fileStorage)
        {
            repository = _repository;
            fileStorage = _fileStorage;
        }

        public async Task<Result<bool>> Handle(DeleteCollegeCommand request, CancellationToken cancellationToken)
        {
            var college = await repository.GetByIdAsync(request.Id);

            // ✅ استخدام null-coalescing operator للتعامل مع القيمة الفارغة
            int imageId = college.ImgId ?? 0;

            var deleteImageResult = await fileStorage.DeleteCollegeImageAsync(college.Id, imageId, "colleges");

            if (!deleteImageResult.IsSuccess)
            {
                return Result<bool>.Failure(deleteImageResult.Message, deleteImageResult.StatusCode);
            }
            return deleteImageResult;
        }
    }
}

