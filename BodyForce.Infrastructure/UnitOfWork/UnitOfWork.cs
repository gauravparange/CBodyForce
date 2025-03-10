using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyForce
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BodyForceDbContext _context;
        private readonly Dictionary<Type, object> _repositories = new();

        public UnitOfWork(BodyForceDbContext context)
        {
            _context = context;
        }

        public IRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T);
            if (!_repositories.ContainsKey(type))
            {
                var repositoryInstance = new Repository<T>(_context);
                _repositories[type] = repositoryInstance;
            }
            return (IRepository<T>)_repositories[type];
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
