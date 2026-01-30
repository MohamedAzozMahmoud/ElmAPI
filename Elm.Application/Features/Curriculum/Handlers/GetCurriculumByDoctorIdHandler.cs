using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Curriculum.DTOs;
using Elm.Application.Contracts.Features.Curriculum.Queries;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.QuestionsBank.Handlers
{
    public sealed class GetCurriculumByDoctorIdHandler : IRequestHandler<GetCurriculumByDoctorIdQuery, Result<List<GetCurriculumDto>>>
    {
        private readonly ICurriculumRepository repository;
        public GetCurriculumByDoctorIdHandler(ICurriculumRepository repository)
        {
            this.repository = repository;
        }
        public async Task<Result<List<GetCurriculumDto>>> Handle(GetCurriculumByDoctorIdQuery request, CancellationToken cancellationToken)
        {
            var curriculums = await repository.GetByDoctorIdAsync(request.doctorId);
            if (curriculums == null)
            {
                return Result<List<GetCurriculumDto>>.Failure("Curriculum not found");
            }
            return Result<List<GetCurriculumDto>>.Success(curriculums);
        }
    }
}
