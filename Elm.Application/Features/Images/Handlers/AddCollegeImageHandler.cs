using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.Files;
using Elm.Application.Contracts.Features.Images.Commands;
using Elm.Application.Contracts.Repositories;
using Elm.Domain.Entities;
using MediatR;


namespace Elm.Application.Features.Images.Handlers
{
    public sealed class AddCollegeImageHandler : IRequestHandler<AddCollegeImageCommand, Result<bool>>
    {
        private readonly ICollegeRepository genericRepository;
        private readonly IFileStorageService fileStorageService;
        public AddCollegeImageHandler(ICollegeRepository genericRepository, IFileStorageService fileStorageService)
        {
            this.genericRepository = genericRepository;
            this.fileStorageService = fileStorageService;
        }
        public async Task<Result<bool>> Handle(AddCollegeImageCommand request, CancellationToken cancellationToken)
        {
            var imagePath = await fileStorageService.UploadImageAsync(request.File, "Images");
            if (!imagePath.IsSuccess || imagePath.Data == null)
            {
                return Result<bool>.Failure("Image upload failed", 500);
            }
            var college = await genericRepository.GetByIdAsync(request.id);
            if (college == null)
            {
                return Result<bool>.Failure("College not found", 404);
            }
            college.Img = new Image
            {
                Name = request.File.FileName,
                ContentType = request.File.ContentType,
                StorageName = Path.GetFileName(imagePath.Data)

            };
            await genericRepository.UpdateAsync(college);
            return Result<bool>.Success(true);
        }
    }
}
