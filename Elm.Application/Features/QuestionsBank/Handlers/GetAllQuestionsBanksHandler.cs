using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.QuestionsBank.DTOs;
using Elm.Application.Contracts.Features.QuestionsBank.Queries;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.QuestionsBank.Handlers
{
    public sealed class GetAllQuestionsBanksHandler : IRequestHandler<GetAllQuestionsBankQuery, Result<List<QuestionsBankDto>>>
    {
        private readonly IQuestionBankRepository repository;
        public GetAllQuestionsBanksHandler(IQuestionBankRepository repository)
        {
            this.repository = repository;
        }
        public async Task<Result<List<QuestionsBankDto>>> Handle(GetAllQuestionsBankQuery request, CancellationToken cancellationToken)
        {
            var questionsBanks = await repository.GetQuestionsBank(request.curriculumId);
            if (!questionsBanks.IsSuccess || questionsBanks.Data == null)
            {
                return Result<List<QuestionsBankDto>>.Failure(questionsBanks.Message);
            }
            return Result<List<QuestionsBankDto>>.Success(questionsBanks.Data);
        }
    }
}
