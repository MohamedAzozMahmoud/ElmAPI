using Elm.Application.Contracts.Features.Authentication.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Domain.Entities;

namespace Elm.Application.Contracts.Abstractions.Files
{
    public interface IDoctorRepository : IGenericRepository<Doctor>
    {
        public Task<Doctor> GetDoctor(string userId);

        public Task<IEnumerable<DoctorDto>> GetAllDoctors(int pageSize, int pageNumber);
    }
}
