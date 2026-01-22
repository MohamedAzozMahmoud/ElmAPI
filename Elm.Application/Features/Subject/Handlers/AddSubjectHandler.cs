using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Subject.Commands;
using Elm.Application.Contracts.Features.Subject.DTOs;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Subject.Handlers
{

    public sealed class AddSubjectHandler : IRequestHandler<AddSubjectCommand, Result<SubjectDto>>
    {
        private readonly ISubjectRepository repository;
        private readonly IMapper mapper;

        public AddSubjectHandler(ISubjectRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
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
            var subjectDto = mapper.Map<SubjectDto>(result);
            return Result<SubjectDto>.Success(subjectDto);
        }
    }
}
