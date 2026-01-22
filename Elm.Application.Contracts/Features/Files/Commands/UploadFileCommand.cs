using MediatR;
using Microsoft.AspNetCore.Http;

namespace Elm.Application.Contracts.Features.Files.Commands
{
    public record UploadFileCommand(int curriculumId, int uploadedById, IFormFile FormFile) : IRequest<Result<string>>;
}
