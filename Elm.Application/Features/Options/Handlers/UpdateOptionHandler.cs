using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Options.Commands;
using Elm.Application.Contracts.Repositories;
using Elm.Domain.Entities;
using MediatR;

namespace Elm.Application.Features.Options.Handlers
{
    public sealed class UpdateOptionHandler : IRequestHandler<UpdateOptionCommand, Result<bool>>
    {
        private readonly IGenericRepository<Option> repository;
        public UpdateOptionHandler(IGenericRepository<Option> repository)
        {
            this.repository = repository;
        }
        public async Task<Result<bool>> Handle(UpdateOptionCommand request, CancellationToken cancellationToken)
        {
            var option = await repository.GetByIdAsync(request.optionId);
            if (option == null)
            {
                return Result<bool>.Failure("Option not found", 404);
            }
            option.Content = request.content;
            option.IsCorrect = request.isCorrect;
            var result = await repository.UpdateAsync(option);
            if (!result)
            {
                return Result<bool>.Failure("Failed to update option", 500);
            }
            return Result<bool>.Success(result);
        }
    }
}
