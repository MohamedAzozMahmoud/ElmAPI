using Elm.Application.Contracts.Features.Curriculum.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Elm.Infrastructure.Repositories
{
    public class CurriculumRepository : GenericRepository<Curriculum>, ICurriculumRepository
    {
        private readonly AppDbContext context;
        private readonly int currentMonth = DateTime.Now.Month;
        public CurriculumRepository(AppDbContext _context) : base(_context)
        {
            context = _context;
        }
        //private bool IsActive()
        //{
        //    if (!IsPublished) return false;

        //    int currentMonth = DateTime.Now.Month;

        //    // إذا كان الترم داخل نفس السنة (مثلاً من شهر 2 لشهر 6)
        //    if (StartMonth <= EndMonth)
        //    {
        //        return currentMonth >= StartMonth && currentMonth <= EndMonth;
        //    }

        //    // إذا كان الترم يعبر السنة (مثلاً من شهر 10 لشهر 2)
        //    // يكون نشطاً إذا كنا (أكبر من شهر البداية) "أو" (أصغر من شهر النهاية)
        //    return currentMonth >= StartMonth || currentMonth <= EndMonth;
        //}

        public async Task<List<GetCurriculumDto>> GetAllCurriculumsByDeptIdAndYearIdAsync(int departmentId, int yearId)
        {
            return await context.Curriculums
                .AsNoTracking()
                .Where(c => c.DepartmentId == departmentId && c.YearId == yearId &&
                         ((c.StartMonth <= c.EndMonth)
                            ? (currentMonth >= c.StartMonth && currentMonth <= c.EndMonth)
                            : (currentMonth >= c.StartMonth || currentMonth <= c.EndMonth)) || c.IsPublished)
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
                .Where(c => c.DoctorId == doctorId && (((c.StartMonth <= c.EndMonth)
                            ? (currentMonth >= c.StartMonth && currentMonth <= c.EndMonth)
                            : (currentMonth >= c.StartMonth || currentMonth <= c.EndMonth)) || c.IsPublished))
                .Select(c => new GetCurriculumDto
                {
                    Id = c.Id,
                    SubjectName = c.Subject.Name
                })
                .ToListAsync();
        }

        public async Task<List<AdminCurriculumDto>> GetBySubjectIdAsync(int subjectId, int pageSize, int pageNumber)
        {
            return await context.Curriculums
                .AsNoTracking()
                .Where(c => c.SubjectId == subjectId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new AdminCurriculumDto
                {
                    Id = c.Id,
                    SubjectName = c.Subject.Name,
                    DepartmentName = c.Department.Name,
                    YearName = c.Year.Name,
                    DoctorName = c.Doctor.User.FullName
                })
                .ToListAsync();
        }
    }
}
