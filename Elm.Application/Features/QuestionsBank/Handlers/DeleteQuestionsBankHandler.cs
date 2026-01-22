using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.QuestionsBank.Commands;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.QuestionsBank.Handlers
{
    public sealed class DeleteQuestionsBankHandler : IRequestHandler<DeleteQuestionsBankCommand, Result<bool>>
    {
        private readonly IGenericRepository<Elm.Domain.Entities.QuestionsBank> repository;
        public DeleteQuestionsBankHandler(IGenericRepository<Elm.Domain.Entities.QuestionsBank> repository)
        {
            this.repository = repository;
        }
        public async Task<Result<bool>> Handle(DeleteQuestionsBankCommand request, CancellationToken cancellationToken)
        {
            var result = await repository.DeleteAsync(request.id);
            if (!result)
            {
                return Result<bool>.Failure("Failed to delete Questions Bank", 500);
            }
            return Result<bool>.Success(true);
        }
    }
}
