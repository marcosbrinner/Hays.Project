using Hays.Domain.Entities;
using System.Linq.Expressions;
namespace Hays.Domain.Abstraction.Repository
{
    public  interface IRepositoryBase<T> where T : BaseEntity
    {
        Task Create(T entity);
        Task Update(T entity);
        Task<IEnumerable<T>> Read(Expression<Func<T, bool>> predicate);
        void Dispose();
    }
}
