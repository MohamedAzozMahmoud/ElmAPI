using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.College.Commands;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.College.Handlers
{
    public sealed class UpdateCollegeHandler : IRequestHandler<UpdateCollegeCommand, Result<bool>>
    {
        private readonly ICollegeRepository repository;
        public UpdateCollegeHandler(ICollegeRepository _repository)
        {
            repository = _repository;
        }
        public async Task<Result<bool>> Handle(UpdateCollegeCommand request, CancellationToken cancellationToken)
        {
            var college = await repository.GetByIdAsync(request.Id);
            if (college == null)
            {
                return Result<bool>.Failure("College not found", 404);
            }
            college.Name = request.Name;
            var result = await repository.UpdateAsync(college);
            if (!result)
            {
                return Result<bool>.Failure("Failed to update college", 500);
            }
            return Result<bool>.Success(true);
        }
    }
}
