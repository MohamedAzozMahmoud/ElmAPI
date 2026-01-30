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

        public async Task<List<GetCurriculumDto>> GetAllCurriculumsByDeptIdAndYearIdAsync(int departmentId, int yearId)
        {
            return await context.Curriculums
                .AsNoTracking()
                .Where(c => c.DepartmentId == departmentId && c.YearId == yearId)
                .Select(c => new GetCurriculumDto
                {
                    Id = c.Id,
                    SubjectName = c.Subject.Name
                })
                .ToListAsync();
        }

        public async Task<List<GetCurriculumDto>> GetByDoctorIdAsync(int doctorId)
        {
            return await context.Curriculums
                .AsNoTracking()
                .Where(c => c.DoctorId == doctorId)
                .Select(c => new GetCurriculumDto
                {
                    Id = c.Id,
                    SubjectName = c.Subject.Name
                })
                .ToListAsync();
        }
    }
}
