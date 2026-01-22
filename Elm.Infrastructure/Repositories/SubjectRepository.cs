using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Subject.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Elm.Infrastructure.Repositories
{
    public class SubjectRepository : GenericRepository<Subject>, ISubjectRepository
    {
        private readonly AppDbContext context;
        public SubjectRepository(AppDbContext _context) : base(_context)
        {
            context = _context;
        }

        public async Task<Result<bool>> ExistsByNameAsync(string name)
        {
            var exists = await context.Subjects.AnyAsync(x => x.Name == name);
            return Result<bool>.Success(exists);
        }

        public async Task<List<GetSubjectDto>> GetAllSubjectByDepartmentIdAsync(int departmentId)
        {
            return await context.Subjects
                .Where(s => s.Curriculums.Select(c => c.DepartmentId).Contains(departmentId))
                .Select(s => new GetSubjectDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Code = s.Code
                })
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
