using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Subject.DTOs;
using Elm.Application.Contracts.Features.Subject.Queries;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Mapper.Elm.Application.Mappers;
using MediatR;

namespace Elm.Application.Features.Subject.Handlers
{
    public sealed class GetSubjectByIdHandler : IRequestHandler<GetSubjectByIdQuery, Result<GetSubjectDto>>
    {
        private readonly ISubjectRepository repository;
        private readonly MappingProvider mapping;

        public GetSubjectByIdHandler(ISubjectRepository repository, MappingProvider mapping)
        {
            this.repository = repository;
            this.mapping = mapping;
        }
        public async Task<Result<GetSubjectDto>> Handle(GetSubjectByIdQuery request, CancellationToken cancellationToken)
        {
            var subject = await repository.GetByIdAsync(request.SubjectId);
            if (subject is null)
            {
                return Result<GetSubjectDto>.Failure("Subject not found.", 404);
            }
            var subjectDto = mapping.MapToGetDto(subject);
            return Result<GetSubjectDto>.Success(subjectDto);
        }
    }
}
