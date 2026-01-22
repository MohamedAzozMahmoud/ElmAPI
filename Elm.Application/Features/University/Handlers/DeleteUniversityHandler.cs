using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.Files;
using Elm.Application.Contracts.Features.University.Commands;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.University.Handlers
{
    public sealed class DeleteUniversityHandler : IRequestHandler<DeleteUniversityCommand, Result<bool>>
    {
        private readonly IGenericRepository<Domain.Entities.University> repository;
        private readonly IFileStorageService fileStorageService;
        public DeleteUniversityHandler(IGenericRepository<Domain.Entities.University> repository,
            IFileStorageService fileStorageService)
        {
            this.repository = repository;
            this.fileStorageService = fileStorageService;
        }
        public async Task<Result<bool>> Handle(DeleteUniversityCommand request, CancellationToken cancellationToken)
        {
            var university = await repository.GetByIdAsync(request.Id);

            // استخدام null-coalescing operator
            int imageId = university.ImgId ?? 0;

            var result = await fileStorageService.DeleteUniversityImageAsync(university.Id, imageId, "Images");

            if (!result.IsSuccess)
            {
                return Result<bool>.Failure("Failed to delete image from storage");
            }
            return Result<bool>.Success(true);
        }
    }
}
