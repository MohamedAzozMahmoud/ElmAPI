using Elm.Application.Contracts.Abstractions.Files;
using Elm.Application.Contracts.Features.Authentication.DTOs;
using Elm.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Elm.Infrastructure.Services.Files
{
    public class StudentRepository : GenericRepository<Domain.Entities.Student>, IStudentRepository
    {
        private readonly AppDbContext context;
        public StudentRepository(AppDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<LeaderDto>> GetAllLeaders(int pageSize, int pageNumber)
        {
            return await context.Students.
                AsNoTracking()
                .Select(x => new LeaderDto
                {
                    FullName = x.User.FullName,
                    UserId = x.AppUserId,
                    DepartmentName = x.Department.Name,
                    YearName = x.Year.Name,
                    UserName = x.User.UserName,
                    IsActived = x.User.IsActived
                })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Domain.Entities.Student> GetLeader(string userId)
        {
            return await context.Students.SingleOrDefaultAsync(s => s.AppUserId == userId) ?? new Domain.Entities.Student();
        }
    }
}
