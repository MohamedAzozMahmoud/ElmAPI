using Elm.Application.Contracts.Features.University.DTOs;
using Elm.Application.Contracts.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Elm.Infrastructure.Repositories
{
    public class UniversityRepository : GenericRepository<Domain.Entities.University>, IUniversityRepository
    {
        private readonly AppDbContext context;

        private readonly IHttpContextAccessor _httpContextAccessor;
        public UniversityRepository(AppDbContext _context, IHttpContextAccessor httpContextAccessor) : base(_context)
        {
            context = _context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UniversityDetialsDto> UniversityDetialsAsync(Expression<Func<Domain.Entities.University, bool>> predicate)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";
            var university = await context.Universities
                .AsNoTracking()
                .Where(predicate)
                .Include(u => u.Img)
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    imageName = u.Img != null ? u.Img.StorageName : null,
                    ImgPath = u.Img != null ? u.Img.FilePath : null
                })
                .FirstOrDefaultAsync();
            return new UniversityDetialsDto
            {
                Id = university.Id,
                Name = university.Name,
                StorageName = university.imageName,
                URL = (university.ImgPath != null)
                      ? $"{baseUrl}/{university.ImgPath.Replace("\\", "/")}" : ""
            };

        }
    }
}
