using Hays.Domain.Abstraction.Repository;

namespace Hays.Domain.Abstraction.DataAcess
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();

        ICustomersRepository CustomersRepository { get; }
    }
}
