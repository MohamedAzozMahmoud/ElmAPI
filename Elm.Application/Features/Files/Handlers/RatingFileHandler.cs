using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.Files;
using Elm.Application.Contracts.Features.Files.Commands;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Files.Handlers
{
    // ApprovedFileCommand
    public sealed class RatingFileHandler : IRequestHandler<RatingFileCommand, Result<bool>>
    {
        private readonly IFileStorageService fileStorage;
        private readonly IGenericRepository<Domain.Entities.Files> filesRepository;
        public RatingFileHandler(IFileStorageService fileStorage, IGenericRepository<Domain.Entities.Files> filesRepository)
        {
            this.fileStorage = fileStorage;
            this.filesRepository = filesRepository;
        }
        public async Task<Result<bool>> Handle(RatingFileCommand request, CancellationToken cancellationToken)
        {
            var file = await filesRepository.GetByIdAsync(request.fileId);
            if (file == null)
            {
                return Result<bool>.Failure("File not found", 404);
            }
            return await fileStorage.RatingFileAsync(file.CurriculumId, request.ratedByDoctorId, request.fileId, request.rating, request.comment);
        }
    }
}
