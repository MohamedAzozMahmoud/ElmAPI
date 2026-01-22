using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Year.DTOs;
using Elm.Application.Contracts.Features.Year.Queries;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Year.Handlers
{
    public sealed class GetYearByIdHandler : IRequestHandler<GetYearByIdQuery, Result<YearDto>>
    {
        private readonly IYearRepository yearRepository;
        private readonly IMapper mapper;
        public GetYearByIdHandler(IYearRepository yearRepository, IMapper mapper)
        {
            this.yearRepository = yearRepository;
            this.mapper = mapper;
        }
        public async Task<Result<YearDto>> Handle(GetYearByIdQuery request, CancellationToken cancellationToken)
        {
            var years = await yearRepository.GetByIdAsync(request.Id);
            var yearDtos = mapper.Map<YearDto>(years);
            return Result<YearDto>.Success(yearDtos);
        }
    }
}
