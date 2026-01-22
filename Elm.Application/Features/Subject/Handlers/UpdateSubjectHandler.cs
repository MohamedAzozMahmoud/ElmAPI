using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Subject.Commands;
using Elm.Application.Contracts.Features.Subject.DTOs;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Subject.Handlers
{
    public sealed class UpdateSubjectHandler : IRequestHandler<UpdateSubjectCommand, Result<SubjectDto>>
    {
        private readonly ISubjectRepository repository;
        private readonly IMapper mapper;
        public UpdateSubjectHandler(ISubjectRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Result<SubjectDto>> Handle(UpdateSubjectCommand request, CancellationToken cancellationToken)
        {
            var subject = await repository.GetByIdAsync(request.Id);
            if (subject is null)
            {
                return Result<SubjectDto>.Failure("Subject not found.");
            }
            subject.Name = request.Name;
            subject.Code = request.Code;
            var result = await repository.UpdateAsync(subject);
            if (result is null)
            {
                return Result<SubjectDto>.Failure("Failed to update subject.");
            }
            var subjectDto = mapper.Map<SubjectDto>(result);
            return Result<SubjectDto>.Success(subjectDto);
        }
    }
}
