using Elm.Application.Contracts.Features.Files.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.Files.Queries
{
    public record GetAllFilesByCurriculumIdQuery(int curriculumId) : IRequest<Result<List<FileView>>>;
}
