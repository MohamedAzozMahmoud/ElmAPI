using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.College.DTOs;
using Elm.Application.Contracts.Features.College.Queries;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.College.Handlers
{
    public sealed class GetCollegeByIdHandler : IRequestHandler<GetCollegeByIdQuery, Result<GetCollegeDto>>
    {
        private readonly ICollegeRepository _collegeRepository;
        private readonly IMapper _mapper;
        public GetCollegeByIdHandler(ICollegeRepository collegeRepository, IMapper mapper)
        {
            _collegeRepository = collegeRepository;
            _mapper = mapper;
        }
        public async Task<Result<GetCollegeDto>> Handle(GetCollegeByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var college = await _collegeRepository.GetByIdAsync(request.Id);
                if (college == null)
                {
                    return Result<GetCollegeDto>.Failure("College not found.");
                }
                var collegeDto = _mapper.Map<GetCollegeDto>(college);
                return Result<GetCollegeDto>.Success(collegeDto);
            }
            catch (Exception ex)
            {
                return Result<GetCollegeDto>.Failure($"An error occurred while retrieving the college: {ex.Message}");
            }
        }
    }
}
