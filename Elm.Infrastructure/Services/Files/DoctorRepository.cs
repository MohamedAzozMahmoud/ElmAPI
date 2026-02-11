using Elm.Application.Contracts.Abstractions.Files;
using Elm.Application.Contracts.Features.Authentication.DTOs;
using Elm.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Elm.Infrastructure.Services.Files
{
    public class DoctorRepository : GenericRepository<Domain.Entities.Doctor>, IDoctorRepository
    {
        private readonly AppDbContext context;
        public DoctorRepository(AppDbContext context) : base(context)
        {
            this.context = context;
        }
        public async Task<Domain.Entities.Doctor> GetDoctor(string userId)
        {
            return await context.Doctors.SingleOrDefaultAsync(d => d.AppUserId == userId) ?? new Domain.Entities.Doctor();
        }
        public async Task<IEnumerable<DoctorDto>> GetAllDoctors(int pageSize, int pageNumber)
        {
            return await context.Doctors
                .AsNoTracking()
                .Include(d => d.User)
                .Select(d => new DoctorDto
                {
                    DoctorId = d.Id,
                    FullName = d.User.FullName,
                    UserName = d.User.UserName,
                    IsActived = d.User.IsActived,
                    Title = d.Title,
                    UserId = d.AppUserId
                })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
