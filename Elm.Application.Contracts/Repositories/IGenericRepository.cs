using System.Linq.Expressions;

namespace Elm.Application.Contracts.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<List<T>> GetAllAsync();
        public Task<T> GetByIdAsync(int id);
        public Task<T> AddAsync(T entity);
        public Task<T> UpdateAsync(T entity);
        public Task<T> FindAsync(Expression<Func<T, bool>> predicate);
        public Task<bool> DeleteAsync(int id);
    }
}
