using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.University.Commands;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.University.Handlers
{
    public sealed class UpdateUniversityHandler : IRequestHandler<UpdateUniversityCommand, Result<bool>>
    {
        private readonly IGenericRepository<Domain.Entities.University> repository;
        public UpdateUniversityHandler(IGenericRepository<Domain.Entities.University> repository)
        {
            this.repository = repository;
        }
        public async Task<Result<bool>> Handle(UpdateUniversityCommand request, CancellationToken cancellationToken)
        {
            var university = await repository.GetByIdAsync(request.Id);
            if (university == null)
            {
                return Result<bool>.Failure("University not found", 404);
            }
            university.Name = request.Name;
            var result = await repository.UpdateAsync(university);
            if (!result)
            {
                return Result<bool>.Failure("Failed to update university");
            }
            return Result<bool>.Success(result);
        }
    }
}
