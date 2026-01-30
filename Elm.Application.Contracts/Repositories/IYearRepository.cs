using Elm.Application.Contracts.Features.Year.DTOs;
using Elm.Domain.Entities;

namespace Elm.Application.Contracts.Repositories
{

    public interface IYearRepository : IGenericRepository<Year>
    {
        public Task<List<GetYearDto>> GetAllYearInCollegeAsync(int collegeId);
        public Task<GetYearDto> GetYearByIdAsync(int yearId);
    }
}
