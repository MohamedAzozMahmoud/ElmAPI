using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Curriculum.DTOs;
using Elm.Application.Contracts.Features.Curriculum.Queries;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.QuestionsBank.Handlers
{
    public sealed class GetAllCurriculumsHandler : IRequestHandler<GetAllCurriculumQuery, Result<List<GetCurriculumDto>>>
    {
        private readonly ICurriculumRepository repository;
        public GetAllCurriculumsHandler(ICurriculumRepository repository)
        {
            this.repository = repository;
        }
        public async Task<Result<List<GetCurriculumDto>>> Handle(GetAllCurriculumQuery request, CancellationToken cancellationToken)
        {
            var curriculums = await repository.GetAllCurriculumsByDeptIdAndYearIdAsync(request.departmentId, request.yearId);
            return Result<List<GetCurriculumDto>>.Success(curriculums);

        }
    }
}
