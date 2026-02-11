using Elm.Application.Contracts.Features.University.DTOs;
using System.Linq.Expressions;

namespace Elm.Application.Contracts.Repositories
{
    public interface IUniversityRepository : IGenericRepository<Domain.Entities.University>
    {
        public Task<UniversityDetialsDto> UniversityDetialsAsync(Expression<Func<Domain.Entities.University, bool>> predicate);
    }
}
