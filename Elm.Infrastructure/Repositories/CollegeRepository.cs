using Elm.Application.Contracts.Features.College.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Elm.Infrastructure.Repositories
{
    public class CollegeRepository : GenericRepository<College>, ICollegeRepository
    {
        private readonly AppDbContext context;
        public CollegeRepository(AppDbContext _context) : base(_context)
        {
            context = _context;
        }

        public async Task<List<GetCollegeDto>> GetAllCollegeInUniversityAsync(int universityId)
        {
            return await context.Colleges
                .AsNoTracking()
                .Where(c => c.UniversityId == universityId)
                .Select(c => new GetCollegeDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    ImagName = c.Img.StorageName
                })
                .ToListAsync();
        }

        public async Task<CollegeDto> GetCollegeByIdAsync(int Id)
        {
            return await context.Colleges
                .AsNoTracking()
                .Where(c => c.Id == Id)
                .Select(c => new CollegeDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .FirstOrDefaultAsync();
        }
    }
}
