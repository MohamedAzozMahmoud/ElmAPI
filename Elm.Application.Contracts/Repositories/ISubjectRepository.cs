using Elm.Application.Contracts.Features.Subject.DTOs;
using Elm.Domain.Entities;

namespace Elm.Application.Contracts.Repositories
{
    public interface ISubjectRepository : IGenericRepository<Subject>
    {
        public Task<List<GetSubjectDto>> GetAllSubjectByDepartmentIdAsync(int departmentId);
        // ExistsByNameAsync
        public Task<Result<bool>> ExistsByNameAsync(string name);
    }
}
