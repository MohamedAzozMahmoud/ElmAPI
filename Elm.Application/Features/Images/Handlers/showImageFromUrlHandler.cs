using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.Files;
using Elm.Application.Contracts.Features.Images.DTOs;
using Elm.Application.Contracts.Features.Images.Queries;
using MediatR;


namespace Elm.Application.Features.Images.Handlers
{
    public sealed class showImageFromUrlHandler : IRequestHandler<showImageFromUrlCommand, Result<ImageDto>>
    {
        private readonly IFileStorageService fileStorageService;
        public showImageFromUrlHandler(IFileStorageService fileStorageService)
        {
            this.fileStorageService = fileStorageService;
        }
        public async Task<Result<ImageDto>> Handle(showImageFromUrlCommand request, CancellationToken cancellationToken)
        {
            var result = await fileStorageService.GetFileAsync(request.FileName, "Images");
            if (!result.IsSuccess || result.Data == null)
            {
                return Result<ImageDto>.Failure("Image not found", 404);
            }
            return Result<ImageDto>.Success(result.Data);
        }
    }
}
