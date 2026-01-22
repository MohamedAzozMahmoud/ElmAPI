using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.Files;
using Elm.Application.Contracts.Features.Files.DTOs;
using Elm.Application.Contracts.Features.Files.Queries;
using MediatR;

namespace Elm.Application.Features.Files.Handlers
{
    public sealed class GetAllFilesByCurriculumIdHandler : IRequestHandler<GetAllFilesByCurriculumIdQuery, Result<List<FileView>>>
    {
        private readonly IFileStorageService fileStorage;
        public GetAllFilesByCurriculumIdHandler(IFileStorageService _fileStorage)
        {
            fileStorage = _fileStorage;
        }
        public async Task<Result<List<FileView>>> Handle(GetAllFilesByCurriculumIdQuery request, CancellationToken cancellationToken)
        {
            return await fileStorage.GetAllFilesByCurriculumIdAsync(request.curriculumId, "Files");
        }
    }
}
