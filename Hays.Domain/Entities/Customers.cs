namespace Hays.Domain.Entities
{
    public class Customers : BaseEntity
    {

        public Customers()
        {
            Name = String.Empty;
            Surname = String.Empty;
            Email = String.Empty;
            Password = String.Empty;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
