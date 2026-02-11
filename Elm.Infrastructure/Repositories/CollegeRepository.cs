using Elm.Application.Contracts.Features.College.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Elm.Infrastructure.Repositories
{
    public class CollegeRepository : GenericRepository<College>, ICollegeRepository
    {
        private readonly AppDbContext context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CollegeRepository(AppDbContext _context, IHttpContextAccessor httpContextAccessor) : base(_context)
        {
            context = _context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<GetCollegeDto>> GetAllCollegeInUniversityAsync(int universityId)
        {
            // 1. تجهيز الرابط الأساسي
            var request = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";

            // 2. جلب البيانات من قاعدة البيانات أولاً (بدون بناء الرابط هنا لتجنب مشاكل EF)
            var collegesData = await context.Colleges
                .AsNoTracking()
                .Where(c => c.UniversityId == universityId)
                // نجلب فقط ما نحتاج لبناء الرابط
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    imageName = c.Img != null ? c.Img.StorageName : null,
                    ImgPath = c.Img != null ? c.Img.FilePath : null
                })
                .ToListAsync();

            // 3. بناء القائمة النهائية في الذاكرة (سريع جداً ولن يؤثر على الأداء)
            var result = collegesData.Select(c => new GetCollegeDto
            {
                Id = c.Id,
                Name = c.Name,
                StorageName = c.imageName,
                // بناء الرابط هنا آمن 100%
                URL = (c.ImgPath != null)
                      ? $"{baseUrl}/{c.ImgPath.Replace("\\", "/")}"
                      : "" // صورة افتراضية في حالة عدم وجود صورة
            }).ToList();

            return result;
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
