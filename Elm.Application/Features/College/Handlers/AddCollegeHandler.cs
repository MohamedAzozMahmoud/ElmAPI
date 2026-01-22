using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.College.Commands;
using Elm.Application.Contracts.Features.College.DTOs;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.College.Handlers
{
    public sealed class AddCollegeHandler : IRequestHandler<AddCollegeCommand, Result<CollegeDto>>
    {
        private readonly ICollegeRepository repository;
        private readonly IMapper mapper;
        public AddCollegeHandler(ICollegeRepository _repository, IMapper _mapper)
        {
            repository = _repository;
            mapper = _mapper;
        }
        public async Task<Result<CollegeDto>> Handle(AddCollegeCommand request, CancellationToken cancellationToken)
        {
            var college = new Domain.Entities.College
            {
                Name = request.name,
                UniversityId = request.UniversityId
            };
            var addedCollege = await repository.AddAsync(college);
            if (addedCollege != null)
            {
                var collegeDto = mapper.Map<CollegeDto>(addedCollege);
                return Result<CollegeDto>.Success(collegeDto);
            }
            return Result<CollegeDto>.Failure("Failed to add college");
        }
    }
}
