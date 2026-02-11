using Elm.Application.Contracts.Features.Authentication.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Domain.Entities;

namespace Elm.Application.Contracts.Abstractions.Files
{
    public interface IStudentRepository : IGenericRepository<Student>
    {
        public Task<IEnumerable<LeaderDto>> GetAllLeaders(int pageSize, int pageNumber);
        public Task<Student> GetLeader(string userId);
    }
}
