using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Curriculum.Commands;
using Elm.Application.Contracts.Features.Curriculum.DTOs;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.QuestionsBank.Handlers
{
    public sealed class UpdateCurriculumHandler : IRequestHandler<UpdateCurriculumCommand, Result<CurriculumDto>>
    {
        private readonly ICurriculumRepository repository;
        private readonly IMapper mapper;
        public UpdateCurriculumHandler(ICurriculumRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Result<CurriculumDto>> Handle(UpdateCurriculumCommand request, CancellationToken cancellationToken)
        {
            var existingCurriculum = await repository.GetByIdAsync(request.Id);
            if (existingCurriculum == null)
            {
                return Result<CurriculumDto>.Failure("Curriculum not found", 404);
            }
            existingCurriculum.SubjectId = request.SubjectId;
            existingCurriculum.YearId = request.YearId;
            existingCurriculum.DepartmentId = request.DepartmentId;
            existingCurriculum.DoctorId = request.DoctorId;
            var updatedCurriculum = await repository.UpdateAsync(existingCurriculum);
            var curriculumDto = mapper.Map<CurriculumDto>(updatedCurriculum);
            return Result<CurriculumDto>.Success(curriculumDto);
        }
    }
}
