using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.QuestionsBank.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Elm.Infrastructure.Repositories
{
    public class QuestionBankRepository : GenericRepository<QuestionsBank>, IQuestionBankRepository
    {
        private readonly AppDbContext context;
        public QuestionBankRepository(AppDbContext _context) : base(_context)
        {
            context = _context;
        }

        public async Task<Result<List<QuestionsBankDto>>> GetQuestionsBank(int curriculumId)
        {
            var questionsBanks = await context.QuestionsBanks
                .Where(qb => qb.CurriculumId == curriculumId)
                .Select(qb => new QuestionsBankDto
                {
                    Id = qb.Id,
                    name = qb.Name
                })
                .AsNoTracking()
                .ToListAsync();
            return Result<List<QuestionsBankDto>>.Success(questionsBanks);
        }
    }
}
