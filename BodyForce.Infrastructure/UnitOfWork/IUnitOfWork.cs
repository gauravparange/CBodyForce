
namespace BodyForce
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> CompleteAsync();
        IRepository<T> Repository<T>() where T : class;
    }
}