using Hays.Domain.Abstraction.DataAcess;
using Hays.Domain.Abstraction.Repository;

namespace Hays.Data.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Context _dbContext;
        public ICustomersRepository CustomersRepository { get; set; }

        public UnitOfWork(
            Context dbContext,
            ICustomersRepository customersRepository)
        {
            _dbContext = dbContext;
            CustomersRepository = customersRepository;
        }

        public Task SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }
    }
}
