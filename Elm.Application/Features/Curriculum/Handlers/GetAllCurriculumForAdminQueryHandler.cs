using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Curriculum.DTOs;
using Elm.Application.Contracts.Features.Curriculum.Queries;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.QuestionsBank.Handlers
{
    public sealed class GetAllCurriculumForAdminQueryHandler : IRequestHandler<GetAllCurriculumForAdminQuery, Result<List<AdminCurriculumDto>>>
    {
        private readonly ICurriculumRepository repository;
        public GetAllCurriculumForAdminQueryHandler(ICurriculumRepository repository)
        {
            this.repository = repository;
        }
        public async Task<Result<List<AdminCurriculumDto>>> Handle(GetAllCurriculumForAdminQuery request, CancellationToken cancellationToken)
        {
            var curriculums = await repository.GetBySubjectIdAsync(request.subjectId, request.pageSize, request.pageNumber);
            return Result<List<AdminCurriculumDto>>.Success(curriculums);
        }
    }
}
