using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.Files;
using Elm.Application.Contracts.Features.Images.Commands;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Images.Handlers
{
    public sealed class DeleteCollegeImageHandler : IRequestHandler<DeleteCollegeImageCommand, Result<bool>>
    {
        private readonly IFileStorageService fileStorage;
        private readonly IGenericRepository<Domain.Entities.College> collegeRepository;

        public DeleteCollegeImageHandler(IFileStorageService _fileStorage, IGenericRepository<Domain.Entities.College> _collegeRepository)
        {
            fileStorage = _fileStorage;
            collegeRepository = _collegeRepository;
        }
        public async Task<Result<bool>> Handle(DeleteCollegeImageCommand request, CancellationToken cancellationToken)
        {
            return await fileStorage.DeleteCollegeImageAsync(request.collegeId, request.id, "Images");
        }
    }
}
