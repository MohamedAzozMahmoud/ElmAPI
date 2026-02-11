using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Subject.DTOs;
using Elm.Application.Contracts.Features.Subject.Queries;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Subject.Handlers
{
    public sealed class GetAllSubjectsQueryHandler : IRequestHandler<GetAllSubjectsQuery, Result<List<GetSubjectDto>>>
    {
        private readonly ISubjectRepository repository;

        public GetAllSubjectsQueryHandler(ISubjectRepository repository)
        {
            this.repository = repository;
        }
        public async Task<Result<List<GetSubjectDto>>> Handle(GetAllSubjectsQuery request, CancellationToken cancellationToken)
        {
            var subjectDtos = await repository.GetAllSubjectAsync(request.pageSize, request.pageNumber);
            return Result<List<GetSubjectDto>>.Success(subjectDtos);
        }
    }
}
