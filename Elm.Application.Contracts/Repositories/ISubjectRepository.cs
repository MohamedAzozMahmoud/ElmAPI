using Elm.Application.Contracts.Features.Subject.DTOs;
using Elm.Domain.Entities;

namespace Elm.Application.Contracts.Repositories
{
    public interface ISubjectRepository : IGenericRepository<Subject>
    {
        public Task<List<GetSubjectDto>> GetAllSubjectByDepartmentIdAsync(int departmentId);
        public Task<List<GetSubjectDto>> GetAllSubjectAsync(int pageSize, int pageNumber);
        // ExistsByNameAsync
        //public Task<Result<bool>> ExistsByNameAsync(string name);
    }
}
