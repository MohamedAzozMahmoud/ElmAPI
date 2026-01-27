using Elm.Application.Contracts.Features.University.DTOs;

namespace Elm.Application.Contracts.Repositories
{
    public interface IUniversityRepository : IGenericRepository<Domain.Entities.University>
    {
        public Task<UniversityDetialsDto> UniversityDetialsAsync(string universityName);
    }
}
