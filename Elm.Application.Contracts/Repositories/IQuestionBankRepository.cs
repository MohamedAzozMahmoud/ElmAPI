using Elm.Application.Contracts.Features.QuestionsBank.DTOs;
using Elm.Domain.Entities;

namespace Elm.Application.Contracts.Repositories
{
    public interface IQuestionBankRepository : IGenericRepository<QuestionsBank>
    {
        public Task<Result<List<QuestionsBankDto>>> GetQuestionsBank(int curriculumId);
    }
}
