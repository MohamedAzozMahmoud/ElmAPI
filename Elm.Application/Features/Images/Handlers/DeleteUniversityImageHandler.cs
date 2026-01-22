using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.Files;
using Elm.Application.Contracts.Features.Images.Commands;
using Elm.Application.Contracts.Repositories;
using MediatR;



namespace Elm.Application.Features.Images.Handlers
{
    public sealed class DeleteUniversityImageHandler : IRequestHandler<DeleteUniversityImageCommand, Result<bool>>
    {
        private readonly IFileStorageService fileStorage;
        private readonly IGenericRepository<Domain.Entities.University> universityRepository;
        public DeleteUniversityImageHandler(IFileStorageService _fileStorage, IGenericRepository<Domain.Entities.University> _universityRepository)
        {
            fileStorage = _fileStorage;
            universityRepository = _universityRepository;
        }
        public async Task<Result<bool>> Handle(DeleteUniversityImageCommand request, CancellationToken cancellationToken)
        {
            return await fileStorage.DeleteUniversityImageAsync(request.universityId, request.id, "Images");
        }
    }
}
