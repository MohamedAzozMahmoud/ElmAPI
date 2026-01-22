using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.College.Commands;
using Elm.Application.Contracts.Features.College.DTOs;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.College.Handlers
{
    public sealed class UpdateCollegeHandler : IRequestHandler<UpdateCollegeCommand, Result<CollegeDto>>
    {
        private readonly ICollegeRepository repository;
        private readonly IMapper mapper;
        public UpdateCollegeHandler(ICollegeRepository _repository, IMapper _mapper)
        {
            repository = _repository;
            mapper = _mapper;
        }
        public async Task<Result<CollegeDto>> Handle(UpdateCollegeCommand request, CancellationToken cancellationToken)
        {
            var college = await repository.GetByIdAsync(request.Id);
            if (college == null)
            {
                return Result<CollegeDto>.Failure("College not found", 404);
            }
            college.Name = request.Name;
            await repository.UpdateAsync(college);
            var updatedCollegeDto = mapper.Map<CollegeDto>(college);
            return Result<CollegeDto>.Success(updatedCollegeDto);
        }
    }
}
