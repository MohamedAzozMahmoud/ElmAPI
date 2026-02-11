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

        public async Task<List<GetDepartmentDto>> GetAllDepartmentsByCollegeIdAsync(int collegeId, int pageNumber, int pageSize)
        {
            return await context.Departments
                .AsNoTracking()
                .Where(d => d.CollegeId == collegeId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(d => new GetDepartmentDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    IsPaid = d.IsPaid,
                    Type = d.Type.ToString()
                })
                .ToListAsync();
        }

        public async Task<List<GetDepartmentDto>> GetAllDepartmentInYearAsync(int yearId)
        {
            return await context.Departments
                .AsNoTracking()
                .Where(d => d.IsPublished && d.Curriculums.Select(c => c.YearId).Contains(yearId))
                .Select(d => new GetDepartmentDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    IsPaid = d.IsPaid,
                    Type = d.Type.ToString()
                })
                .ToListAsync();
        }


    }
}
