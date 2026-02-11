using Elm.Application.Contracts.Features.College.DTOs;
using Elm.Domain.Entities;

namespace Elm.Application.Contracts.Repositories
{
    public interface ICollegeRepository : IGenericRepository<College>
    {
        public Task<List<GetCollegeDto>> GetAllCollegeInUniversityAsync(int universityId);
        public Task<CollegeDto> GetCollegeByIdAsync(int Id);

    }
}
