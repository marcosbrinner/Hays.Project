using Hays.Domain.Entities;
using Hays.Domain.Abstraction.Repository;
using Hays.Data.DataAccess;
using Microsoft.EntityFrameworkCore;
using Hays.Domain.DTO;

namespace Hays.Data.Repositories
{
    public class CustomersRepository : RepositoryBase<Customers>, ICustomersRepository
    {
        public CustomersRepository(Context contexto) : base(contexto)
        {
        }

        public async Task<bool> CehckIfExistsByEmail(string email)
        {
            return await _context.Customers
                .Where(x => x.Email == email.ToLower()).AnyAsync();
        }

        public async Task<IEnumerable<CustomerViewDTO>> GetAll()
        {
            return await _context.Customers
                .Where(x => x.Id != Guid.Empty).Select(x => new CustomerViewDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    Surname = x.Surname
                }).ToListAsync();
        }

        public async Task<string?> GetEmailById(Guid id)
        {
            return await _context.Customers
                .Where(x => x.Id == id).Select(x => x.Email)
                .FirstOrDefaultAsync();
        }

        public async Task<CustomerViewDTO?> GetById(Guid id)
        {
            return await _context.Customers
                .Where(x => x.Id == id)
                .Select(x => new CustomerViewDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    Surname = x.Surname
                }).SingleOrDefaultAsync();
        }

        public async Task<Customers?> GetByIdToEdit(Guid id)
        {
            return await _context.Customers
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync();
        }
    }
}
