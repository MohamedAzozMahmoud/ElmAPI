using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Year.Commands;
using Elm.Application.Contracts.Features.Year.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Mapper.Elm.Application.Mappers;
using MediatR;

namespace Elm.Application.Features.Year.Handlers
{
    public sealed class AddYearHandler : IRequestHandler<AddYearCommand, Result<YearDto>>
    {
        private readonly IYearRepository yearRepository;
        private readonly MappingProvider mapping;
        public AddYearHandler(IYearRepository yearRepository, MappingProvider mapping)
        {
            this.yearRepository = yearRepository;
            this.mapping = mapping;
        }
        public async Task<Result<YearDto>> Handle(AddYearCommand request, CancellationToken cancellationToken)
        {
            var year = new Domain.Entities.Year
            {
                Name = request.name,
                CollegeId = request.collegeId
            };
            var addedYear = await yearRepository.AddAsync(year);
            if (addedYear != null)
            {
                var yearDto = mapping.MapToDto(addedYear);
                return Result<YearDto>.Success(yearDto);
            }
            return Result<YearDto>.Failure("Failed to add year");
        }
    }
}
