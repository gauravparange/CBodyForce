
namespace BodyForce
{
    public interface IRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);
        void Delete(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task SaveChangesAsync();
        Task<T> Update(T entity);
    }
}