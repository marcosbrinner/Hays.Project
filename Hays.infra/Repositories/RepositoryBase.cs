using Hays.Domain.Entities;
using Hays.Domain.Abstraction.Repository;
using Hays.Data.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Hays.Data.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T>, IDisposable where T : BaseEntity
    {
        public Context _context;

        public RepositoryBase(Context contexto)
        {
            _context = contexto;
        }

        public async Task Create(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
            GC.SuppressFinalize(this);
        }

        public async Task<IEnumerable<T>> Read(Expression<Func<T, bool>> predicate)
        {
            var resultado = await _context.Set<T>().Where(predicate)
                .ToListAsync();
            return resultado;
        }

    }
}
