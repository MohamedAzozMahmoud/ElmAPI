using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.University.Commands;
using Elm.Application.Contracts.Features.University.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Mapper.Elm.Application.Mappers;
using MediatR;

namespace Elm.Application.Features.University.Handlers
{
    public sealed class AddUniversityHandler : IRequestHandler<AddUniversityCommand, Result<UniversityDto>>
    {
        private readonly IGenericRepository<Elm.Domain.Entities.University> _universityRepository;
        private readonly MappingProvider _mapping;
        public AddUniversityHandler(IGenericRepository<Elm.Domain.Entities.University> universityRepository, MappingProvider mapping)
        {
            _universityRepository = universityRepository;
            _mapping = mapping;
        }
        public async Task<Result<UniversityDto>> Handle(AddUniversityCommand request, CancellationToken cancellationToken)
        {
            var university = new Elm.Domain.Entities.University
            {
                Name = request.Name
            };
            await _universityRepository.AddAsync(university);
            var universityDto = _mapping.MapToDto(university);
            return Result<UniversityDto>.Success(universityDto);
        }
    }
}
