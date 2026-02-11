using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Year.Commands;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Year.Handlers
{
    public sealed class UpdateYearHandler : IRequestHandler<UpdateYearCommand, Result<bool>>
    {
        private readonly IYearRepository yearRepository;

        public UpdateYearHandler(IYearRepository yearRepository)
        {
            this.yearRepository = yearRepository;
        }
        public async Task<Result<bool>> Handle(UpdateYearCommand request, CancellationToken cancellationToken)
        {
            var year = await yearRepository.GetByIdAsync(request.Id);
            if (year == null)
            {
                return Result<bool>.NotFound("Year not found");
            }
            year.Name = request.Name;
            var updatedYear = await yearRepository.UpdateAsync(year);
            if (updatedYear)
            {
                return Result<bool>.Success(true);
            }
            return Result<bool>.Failure("Failed to update year");
        }
    }
}
