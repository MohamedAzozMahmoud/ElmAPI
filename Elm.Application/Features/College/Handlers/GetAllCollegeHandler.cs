using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.College.DTOs;
using Elm.Application.Contracts.Features.College.Queries;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.College.Handlers
{
    public sealed class GetAllCollegeHandler : IRequestHandler<GetAllCollegeQuery, Result<List<GetCollegeDto>>>
    {
        private readonly ICollegeRepository _collegeRepository;
        public GetAllCollegeHandler(ICollegeRepository collegeRepository)
        {
            _collegeRepository = collegeRepository;
        }
        public async Task<Result<List<GetCollegeDto>>> Handle(GetAllCollegeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var colleges = await _collegeRepository.GetAllCollegeInUniversityAsync(request.universityId);
                return Result<List<GetCollegeDto>>.Success(colleges);
            }
            catch (Exception ex)
            {
                return Result<List<GetCollegeDto>>.Failure($"An error occurred while retrieving colleges: {ex.Message}");
            }
        }
    }
}
