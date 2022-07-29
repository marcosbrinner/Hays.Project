using Hays.Domain.DTO;
using Hays.Domain.Entities;
using Hays.Domain.Models;

namespace Hays.Domain.Abstraction.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerViewDTO>> GetAll();
        Task<CustomerViewDTO?> GetById(Guid id);
        Task<Bag<Customers>> CretaeNewCustomer(CustomerDTO newCustomer);
        Task<Bag<Customers>> UpdateCustomer(CustomerDTO newCustomer, Guid customerId);
    }
}
