using Elm.Application.Contracts.Features.Year.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Elm.Infrastructure.Repositories
{
    public class YearRepository : GenericRepository<Year>, IYearRepository
    {
        private readonly AppDbContext context;
        public YearRepository(AppDbContext _context) : base(_context)
        {
            context = _context;
        }

        public async Task<List<GetYearDto>> GetAllYearInCollegeAsync(int collegeId)
        {
            return await context.Years
                .AsNoTracking()
                .Where(y => y.CollegeId == collegeId)
                .Select(y => new GetYearDto
                { Id = y.Id, Name = y.Name })
                .ToListAsync();
        }

        public async Task<GetYearDto> GetYearByIdAsync(int yearId)
        {
            return await context.Years
                .AsNoTracking()
                .Where(y => y.Id == yearId)
                .Select(y => new GetYearDto
                { Id = y.Id, Name = y.Name })
                .FirstOrDefaultAsync() ?? new GetYearDto();
        }
    }
}
