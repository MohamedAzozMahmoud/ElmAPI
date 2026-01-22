using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Year.Commands;
using Elm.Application.Contracts.Features.Year.DTOs;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Year.Handlers
{
    public sealed class UpdateYearHandler : IRequestHandler<UpdateYearCommand, Result<YearDto>>
    {
        private readonly IYearRepository yearRepository;
        private readonly IMapper mapper;
        public UpdateYearHandler(IYearRepository yearRepository, IMapper mapper)
        {
            this.yearRepository = yearRepository;
            this.mapper = mapper;
        }
        public async Task<Result<YearDto>> Handle(UpdateYearCommand request, CancellationToken cancellationToken)
        {
            var year = mapper.Map<Domain.Entities.Year>(request);
            var updatedYear = await yearRepository.UpdateAsync(year);
            if (updatedYear != null)
            {
                var yearDto = mapper.Map<YearDto>(updatedYear);
                return Result<YearDto>.Success(yearDto);
            }
            return Result<YearDto>.Failure("Failed to update year");
        }
    }
}
