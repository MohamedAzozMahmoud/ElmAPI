using Elm.Application.Contracts.Features.Department.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Elm.Infrastructure.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        private readonly AppDbContext context;
        public DepartmentRepository(AppDbContext _context) : base(_context)
        {
            context = _context;
        }

        public async Task<List<GetDepartmentDto>> GetAllDepartmentInCollegeAsync(int yearId)
        {
            return await context.Departments
                .Where(d => d.Curriculums.Select(c => c.YearId).Contains(yearId))
                .Select(d => new GetDepartmentDto
                {
                    Id = d.Id,
                    Name = d.Name
                })
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
