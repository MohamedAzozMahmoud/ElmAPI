using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.Files;
using Elm.Application.Contracts.Features.Images.Commands;
using MediatR;



namespace Elm.Application.Features.Images.Handlers
{
    public sealed class DeleteImageByNameCommandHandler : IRequestHandler<DeleteImageByNameCommand, Result<bool>>
    {
        private readonly IFileStorageService fileStorage;
        public DeleteImageByNameCommandHandler(IFileStorageService _fileStorage)
        {
            fileStorage = _fileStorage;
        }
        public async Task<Result<bool>> Handle(DeleteImageByNameCommand request, CancellationToken cancellationToken)
        {
            return await fileStorage.DeleteImageAsync(request.fileName);
        }
    }
}
