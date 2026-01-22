using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Curriculum.Commands;
using Elm.Application.Contracts.Features.Curriculum.DTOs;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.QuestionsBank.Handlers
{
    public sealed class AddCurriculumHandler : IRequestHandler<AddCurriculumCommand, Result<CurriculumDto>>
    {
        private readonly ICurriculumRepository repository;
        private readonly IMapper mapper;

        public AddCurriculumHandler(ICurriculumRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Result<CurriculumDto>> Handle(AddCurriculumCommand request, CancellationToken cancellationToken)
        {
            var curriculum = new Elm.Domain.Entities.Curriculum
            {
                SubjectId = request.SubjectId,
                YearId = request.YearId,
                DepartmentId = request.DepartmentId,
                DoctorId = request.DoctorId
            };
            var addedCurriculum = await repository.AddAsync(curriculum);
            var curriculumDto = mapper.Map<CurriculumDto>(addedCurriculum);
            return Result<CurriculumDto>.Success(curriculumDto);
        }
    }
}
