using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Curriculum.DTOs;
using Elm.Application.Contracts.Features.Curriculum.Queries;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Mapper.Elm.Application.Mappers;
using MediatR;

namespace Elm.Application.Features.QuestionsBank.Handlers
{
    public sealed class GetCurriculumByIdHandler : IRequestHandler<GetCurriculumByIdQuery, Result<CurriculumDto>>
    {
        private readonly ICurriculumRepository repository;
        private readonly MappingProvider _mapping;
        public GetCurriculumByIdHandler(ICurriculumRepository repository, MappingProvider mapping)
        {
            this.repository = repository;
            _mapping = mapping;
        }
        public async Task<Result<CurriculumDto>> Handle(GetCurriculumByIdQuery request, CancellationToken cancellationToken)
        {
            var curriculum = await repository.GetByIdAsync(request.Id);
            if (curriculum == null)
            {
                return Result<CurriculumDto>.Failure("Curriculum not found");
            }
            var curriculumDto = _mapping.MapToDto(curriculum);
            return Result<CurriculumDto>.Success(curriculumDto);
        }
    }
}
