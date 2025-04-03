
using System.Linq.Expressions;

namespace BodyForce
{
    public interface IRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);
        void Delete(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetByConditionAsync(Expression<Func<T, bool>> expression);
        Task<T> GetByIdAsync(int id);
        Task SaveChangesAsync();
        Task<T> Update(T entity);
    }
}