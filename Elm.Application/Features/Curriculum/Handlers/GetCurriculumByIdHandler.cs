using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Curriculum.DTOs;
using Elm.Application.Contracts.Features.Curriculum.Queries;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.QuestionsBank.Handlers
{
    public sealed class GetCurriculumByIdHandler : IRequestHandler<GetCurriculumByIdQuery, Result<CurriculumDto>>
    {
        private readonly ICurriculumRepository repository;
        private readonly IMapper mapper;
        public GetCurriculumByIdHandler(ICurriculumRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Result<CurriculumDto>> Handle(GetCurriculumByIdQuery request, CancellationToken cancellationToken)
        {
            var curriculum = await repository.GetByIdAsync(request.Id);
            if (curriculum == null)
            {
                return Result<CurriculumDto>.Failure("Curriculum not found");
            }
            var curriculumDto = mapper.Map<CurriculumDto>(curriculum);
            return Result<CurriculumDto>.Success(curriculumDto);
        }
    }
}
