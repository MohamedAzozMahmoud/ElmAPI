using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Questions.Commands;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Questions.Handlers
{
    public sealed class DeleteQuestionHandler : IRequestHandler<DeleteQuestionCommand, Result<bool>>
    {
        private readonly IQuestionRepository repository;
        public DeleteQuestionHandler(IQuestionRepository _repository)
        {
            repository = _repository;
        }
        public async Task<Result<bool>> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
        {
            var result = await repository.DeleteAsync(request.questionId);
            if (!result)
            {
                return Result<bool>.Failure("Question not found", 404);
            }
            return Result<bool>.Success(true);
        }
    }
}
