using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.University.Commands;
using Elm.Application.Contracts.Features.University.DTOs;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.University.Handlers
{
    public sealed class AddUniversityHandler : IRequestHandler<AddUniversityCommand, Result<UniversityDto>>
    {
        private readonly IGenericRepository<Elm.Domain.Entities.University> _universityRepository;
        private readonly IMapper _mapper;
        public AddUniversityHandler(IGenericRepository<Elm.Domain.Entities.University> universityRepository, IMapper mapper)
        {
            _universityRepository = universityRepository;
            _mapper = mapper;
        }
        public async Task<Result<UniversityDto>> Handle(AddUniversityCommand request, CancellationToken cancellationToken)
        {
            var university = new Elm.Domain.Entities.University
            {
                Name = request.Name
            };
            await _universityRepository.AddAsync(university);
            var universityDto = _mapper.Map<UniversityDto>(university);
            return Result<UniversityDto>.Success(universityDto);
        }
    }
}
