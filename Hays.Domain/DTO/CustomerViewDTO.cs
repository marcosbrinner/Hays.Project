using Hays.Domain.Entities;

namespace Hays.Domain.DTO
{
    public class CustomerViewDTO : BaseEntity
    {
        public CustomerViewDTO()
        {
            Name = String.Empty;
            Surname = String.Empty;
            Email = String.Empty;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
    }
}
