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
            if (university == null)
            {
                return Result<bool>.Failure("الجامعة غير موجودة", 404);
            }
            var result = await fileStorageService.DeleteUniversityAsync(university.Id);

            if (!result.IsSuccess)
            {
                return Result<bool>.Failure("فشل في حذف الصورة من التخزين", 500);
            }
            return Result<bool>.Success(true);
        }
    }
}
