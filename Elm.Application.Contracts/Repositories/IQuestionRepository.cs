using Elm.Application.Contracts.Features.Questions.DTOs;
using Elm.Application.Contracts.Features.Test.DTOs;
using Elm.Domain.Entities;

namespace Elm.Application.Contracts.Repositories
{
    public interface IQuestionRepository : IGenericRepository<Question>
    {
        public Task<Result<QuestionsDto>> GetQuestionById(int questionId);
        public Task<Result<List<QuestionsDto>>> GetQuestionsByBankId(int questionsBankId);
        public Task<Result<bool>> AddRingQuestions(int questionsBankId, List<AddQuestionsDto> questionsDtos);
        public Task<Result<bool>> AddRingQuestionsFromExcel(int questionsBankId, List<TemplateQuestionsDto> templateQuestions);

        Task<QuestionBankInfo?> GetBankInfoAsync(int bankId);
        Task<List<int>> GetRandomQuestionIdsAsync(int bankId, int count);
        Task<List<QuestionWithOptions>> GetQuestionsWithOptionsAsync(List<int> questionIds);

    }
}
