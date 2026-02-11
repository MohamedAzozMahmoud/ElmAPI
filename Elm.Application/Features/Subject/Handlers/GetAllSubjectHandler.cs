using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Subject.DTOs;
using Elm.Application.Contracts.Features.Subject.Queries;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Subject.Handlers
{
    public sealed class GetAllSubjectHandler : IRequestHandler<GetAllSubjectByDepartmentIdQuery, Result<List<GetSubjectDto>>>
    {
        private readonly ISubjectRepository repository;
        public GetAllSubjectHandler(ISubjectRepository repository)
        {
            this.repository = repository;
        }
        public async Task<Result<List<GetSubjectDto>>> Handle(GetAllSubjectByDepartmentIdQuery request, CancellationToken cancellationToken)
        {
            var subjects = await repository.GetAllSubjectByDepartmentIdAsync(request.departmentId);
            return Result<List<GetSubjectDto>>.Success(subjects);
        }
    }
}
