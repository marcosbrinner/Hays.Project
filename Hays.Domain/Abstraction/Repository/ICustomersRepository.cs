using Hays.Domain.DTO;
using Hays.Domain.Entities;

namespace Hays.Domain.Abstraction.Repository
{
    public interface ICustomersRepository : IRepositoryBase<Customers>
    {
        Task<bool> CehckIfExistsByEmail(string email);
        Task<CustomerViewDTO?> GetById(Guid id);
        Task<string?> GetEmailById(Guid id);
        Task<IEnumerable<CustomerViewDTO>> GetAll();
        Task<Customers?> GetByIdToEdit(Guid id);
    }
}
