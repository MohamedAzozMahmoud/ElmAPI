using AutoMapper;
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
        private readonly IMapper mapper;
        public GetAllQuestionsBanksHandler(IQuestionBankRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
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
