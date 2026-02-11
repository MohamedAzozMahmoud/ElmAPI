using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.Files;
using Elm.Application.Contracts.Features.Authentication.DTOs;
using Elm.Application.Contracts.Features.Authentication.Queries;
using MediatR;

namespace Elm.Application.Features.Authentication.Handlers
{
    public sealed class GetAllLeadersQueryHandler : IRequestHandler<GetAllLeadersQuery, Result<IEnumerable<LeaderDto>>>
    {
        private readonly IStudentRepository studentRepository;
        public GetAllLeadersQueryHandler(IStudentRepository studentRepository)
        {
            this.studentRepository = studentRepository;
        }
        public async Task<Result<IEnumerable<LeaderDto>>> Handle(GetAllLeadersQuery request, CancellationToken cancellationToken)
        {
            var leaders = await studentRepository.GetAllLeaders(request.pageSize, request.pageNumber);
            return Result<IEnumerable<LeaderDto>>.Success(leaders);
        }
    }
}
