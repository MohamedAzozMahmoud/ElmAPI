using Elm.Application.Contracts.Features.Curriculum.DTOs;
using Elm.Domain.Entities;

namespace Elm.Application.Contracts.Repositories
{
    public interface ICurriculumRepository : IGenericRepository<Curriculum>
    {
        public Task<List<GetCurriculumDto>> GetAllCurriculumsByDeptIdAndYearIdAsync(int departmentId, int yearId);
        public Task<List<GetCurriculumDto>> GetByDoctorIdAsync(int doctorId);
        public Task<List<AdminCurriculumDto>> GetBySubjectIdAsync(int subjectId, int pageSize, int pageNumber);



    }
}