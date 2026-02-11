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
        private readonly IDoctorRepository doctorsRepository;
        public RatingFileHandler(IFileStorageService fileStorage, IGenericRepository<Domain.Entities.Files> filesRepository, IDoctorRepository doctorsRepository)
        {
            this.fileStorage = fileStorage;
            this.filesRepository = filesRepository;
            this.doctorsRepository = doctorsRepository;
        }
        public async Task<Result<bool>> Handle(RatingFileCommand request, CancellationToken cancellationToken)
        {
            var file = await filesRepository.GetByIdAsync(request.fileId);
            if (file == null)
            {
                return Result<bool>.Failure("File not found", 404);
            }
            var doctor = await doctorsRepository.GetDoctor(request.userId);
            if (doctor == null)
            {
                return Result<bool>.Failure("Doctor not found", 404);
            }
            return await fileStorage.RatingFileAsync(file.CurriculumId, doctor.Id, request.fileId, request.rating, request.comment);
        }
    }
}
