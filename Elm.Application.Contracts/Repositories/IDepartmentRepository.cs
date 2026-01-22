using Elm.Application.Contracts.Features.Department.DTOs;
using Elm.Domain.Entities;

namespace Elm.Application.Contracts.Repositories
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        public Task<List<GetDepartmentDto>> GetAllDepartmentInCollegeAsync(int yearId);
    }
}
