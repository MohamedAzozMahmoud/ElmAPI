using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Questions.Commands;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Questions.Handlers
{
    public sealed class AddRingQuestionHandler : IRequestHandler<AddRingQuestionsCommand, Result<bool>>
    {
        private readonly IQuestionRepository repository;
        public AddRingQuestionHandler(IQuestionRepository _repository)
        {
            repository = _repository;
        }
        public async Task<Result<bool>> Handle(AddRingQuestionsCommand request, CancellationToken cancellationToken)
        {
            return await repository.AddRingQuestions(request.questionsBankId, request.QuestionsDtos);
        }
    }
}
