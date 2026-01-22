using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.University.Commands;
using Elm.Application.Contracts.Features.University.DTOs;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.University.Handlers
{
    public sealed class UpdateUniversityHandler : IRequestHandler<UpdateUniversityCommand, Result<UniversityDto>>
    {
        private readonly IGenericRepository<Domain.Entities.University> repository;
        private readonly IMapper mapper;
        public UpdateUniversityHandler(IGenericRepository<Domain.Entities.University> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Result<UniversityDto>> Handle(UpdateUniversityCommand request, CancellationToken cancellationToken)
        {
            var university = await repository.GetByIdAsync(request.Id);
            if (university == null)
            {
                return Result<UniversityDto>.Failure("University not found", 404);
            }
            university.Name = request.Name;
            var updatedUniversity = await repository.UpdateAsync(university);
            var universityDto = mapper.Map<UniversityDto>(updatedUniversity);
            return Result<UniversityDto>.Success(universityDto);
        }
    }
}
