using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Questions.DTOs;
using Elm.Application.Contracts.Features.Questions.Queries;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Questions.Handlers
{
    public sealed class GetQuestionsByBankIdHandler : IRequestHandler<GetAllQuestionsQuery, Result<List<QuestionsDto>>>
    {
        private readonly IQuestionRepository repository;
        public GetQuestionsByBankIdHandler(IQuestionRepository _repository)
        {
            repository = _repository;
        }
        public async Task<Result<List<QuestionsDto>>> Handle(GetAllQuestionsQuery request, CancellationToken cancellationToken)
        {
            var questionsResult = await repository.GetQuestionsByBankId(request.questionBankId);
            if (questionsResult.Data != null)
            {
                return Result<List<QuestionsDto>>.Success(questionsResult.Data);
            }
            return Result<List<QuestionsDto>>.Failure(questionsResult.Message, questionsResult.StatusCode);
        }
    }
}
