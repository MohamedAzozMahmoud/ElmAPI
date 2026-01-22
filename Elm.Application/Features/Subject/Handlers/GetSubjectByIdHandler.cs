using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Subject.DTOs;
using Elm.Application.Contracts.Features.Subject.Queries;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Subject.Handlers
{
    public sealed class GetSubjectByIdHandler : IRequestHandler<GetSubjectByIdQuery, Result<GetSubjectDto>>
    {
        private readonly ISubjectRepository repository;
        private readonly IMapper mapper;

        public GetSubjectByIdHandler(ISubjectRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Result<GetSubjectDto>> Handle(GetSubjectByIdQuery request, CancellationToken cancellationToken)
        {
            var subject = await repository.GetByIdAsync(request.SubjectId);
            if (subject is null)
            {
                return Result<GetSubjectDto>.Failure("Subject not found.", 404);
            }
            var subjectDto = mapper.Map<GetSubjectDto>(subject);
            return Result<GetSubjectDto>.Success(subjectDto);
        }
    }
}
