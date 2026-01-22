using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Year.DTOs;
using Elm.Application.Contracts.Features.Year.Queries;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Year.Handlers
{
    public sealed class GetAllYearHandler : IRequestHandler<GetAllYearQuery, Result<List<GetYearDto>>>
    {
        private readonly IYearRepository yearRepository;
        private readonly IMapper mapper;
        public GetAllYearHandler(IYearRepository yearRepository, IMapper mapper)
        {
            this.yearRepository = yearRepository;
            this.mapper = mapper;
        }

        public async Task<Result<List<GetYearDto>>> Handle(GetAllYearQuery request, CancellationToken cancellationToken)
        {
            var years = await yearRepository.GetAllYearInCollegeAsync(request.collegeId);
            return Result<List<GetYearDto>>.Success(years);
        }
    }
}
