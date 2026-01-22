using Elm.Application.Contracts.Features.Subject.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.Subject.Queries
{
    public record GetSubjectByIdQuery(
        int SubjectId
    ) : IRequest<Result<GetSubjectDto>>;
}
