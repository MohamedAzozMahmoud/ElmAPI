using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Subject.Commands;
using Elm.Application.Contracts.Features.Subject.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Mapper.Elm.Application.Mappers;
using MediatR;

namespace Elm.Application.Features.Subject.Handlers
{

    public sealed class AddSubjectHandler : IRequestHandler<AddSubjectCommand, Result<SubjectDto>>
    {
        private readonly ISubjectRepository repository;
        private readonly MappingProvider mapping;

        public AddSubjectHandler(ISubjectRepository repository, MappingProvider mapping)
        {
            this.repository = repository;
            this.mapping = mapping;
        }
        public async Task<Result<SubjectDto>> Handle(AddSubjectCommand request, CancellationToken cancellationToken)
        {
            var subject = new Elm.Domain.Entities.Subject
            {
                Name = request.Name,
                Code = request.Code
            };
            var result = await repository.AddAsync(subject);
            if (result is null)
            {
                return Result<SubjectDto>.Failure("Failed to add subject.");
            }
            var subjectDto = mapping.MapToDto(result);
            return Result<SubjectDto>.Success(subjectDto);
        }
    }
}
