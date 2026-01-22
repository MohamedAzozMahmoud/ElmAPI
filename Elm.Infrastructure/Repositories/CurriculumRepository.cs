using Elm.Application.Contracts.Features.Curriculum.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Elm.Infrastructure.Repositories
{
    public class CurriculumRepository : GenericRepository<Curriculum>, ICurriculumRepository
    {
        private readonly AppDbContext context;
        public CurriculumRepository(AppDbContext _context) : base(_context)
        {
            context = _context;
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            var exists = await context.Curriculums.AnyAsync(x => x.Subject.Name == name);
            return exists;
        }

        public async Task<List<GetCurriculumDto>> GetAllCurriculumsByDeptIdAndYearIdAsync(int departmentId, int yearId)
        {
            return await context.Curriculums
                .Where(c => c.DepartmentId == departmentId && c.YearId == yearId)
                .Select(c => new GetCurriculumDto
                {
                    Id = c.Id,
                    SubjectName = c.Subject.Name
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<string>> GetFileInfoByIdAsync(int curriculumId)
        {
            return await context.Files
                .Where(f => f.CurriculumId == curriculumId)
                .Select(f => f.StorageName)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
