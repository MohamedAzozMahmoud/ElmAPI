using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.College.DTOs;
using Elm.Application.Contracts.Features.College.Queries;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.College.Handlers
{
    public sealed class GetCollegeByIdHandler : IRequestHandler<GetCollegeByIdQuery, Result<CollegeDto>>
    {
        private readonly ICollegeRepository _collegeRepository;
        private readonly IMapper _mapper;
        public GetCollegeByIdHandler(ICollegeRepository collegeRepository, IMapper mapper)
        {
            _collegeRepository = collegeRepository;
            _mapper = mapper;
        }
        public async Task<Result<CollegeDto>> Handle(GetCollegeByIdQuery request, CancellationToken cancellationToken)
        {
            var collegeDto = await _collegeRepository.GetCollegeByIdAsync(request.Id);
            if (collegeDto == null)
            {
                return Result<CollegeDto>.Failure("College not found.");
            }
            return Result<CollegeDto>.Success(collegeDto);

        }
    }
}
