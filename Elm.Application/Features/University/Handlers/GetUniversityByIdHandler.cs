using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.University.DTOs;
using Elm.Application.Contracts.Features.University.Queries;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.University.Handlers
{
    public sealed class GetUniversityByIdHandler : IRequestHandler<GetUniversityByIdQuery, Result<UniversityDetialsDto>>
    {
        private readonly IUniversityRepository repository;
        public GetUniversityByIdHandler(IUniversityRepository repository)
        {
            this.repository = repository;
        }
        public async Task<Result<UniversityDetialsDto>> Handle(GetUniversityByIdQuery request, CancellationToken cancellationToken)
        {
            var university = await repository.UniversityDetialsAsync(request.Id);
            if (university == null)
            {
                return Result<UniversityDetialsDto>.Failure("University not found", 404);
            }

            return Result<UniversityDetialsDto>.Success(university);
        }
    }
}
