using Elm.Application.Contracts.Features.University.DTOs;
using Elm.Application.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Elm.Infrastructure.Repositories
{
    public class UniversityRepository : GenericRepository<Domain.Entities.University>, IUniversityRepository
    {
        private readonly AppDbContext context;
        public UniversityRepository(AppDbContext _context) : base(_context)
        {
            context = _context;
        }

        public async Task<UniversityDetialsDto> UniversityDetialsAsync(string universityName)
        {
            return await context.Universities
                .AsNoTracking()
                .Where(u => u.Name == universityName)
                .Include(u => u.Img)
                .Select(u => new UniversityDetialsDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    ImageName = u.Img.StorageName
                })
                .SingleOrDefaultAsync(x => x.Name == universityName) ?? new UniversityDetialsDto();
        }
    }
}
