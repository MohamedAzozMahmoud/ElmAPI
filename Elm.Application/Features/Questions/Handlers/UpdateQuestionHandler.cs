using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Questions.Commands;
using Elm.Application.Contracts.Repositories;
using Elm.Domain.Enums;
using MediatR;

namespace Elm.Application.Features.Questions.Handlers
{
    public sealed class UpdateQuestionHandler : IRequestHandler<UpdateQuestionCommand, Result<bool>>
    {
        private readonly IQuestionRepository repository;
        public UpdateQuestionHandler(IQuestionRepository _repository)
        {
            repository = _repository;
        }
        public async Task<Result<bool>> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
        {
            var QuestionResult = await repository.GetByIdAsync(request.Id);
            if (QuestionResult is null)
            {
                return Result<bool>.Failure("Question not found", 404);
            }
            QuestionResult.Content = request.Content;
            QuestionResult.QuestionType = Enum.Parse<QuestionType>(request.QuestionType);

            var result = await repository.UpdateAsync(QuestionResult);
            if (!result)
            {
                return Result<bool>.Failure("Failed to update Question", 500);
            }
            return Result<bool>.Success(result);
        }
    }
}
