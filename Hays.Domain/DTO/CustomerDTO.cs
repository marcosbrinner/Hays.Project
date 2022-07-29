namespace Hays.Domain.DTO
{
    public class CustomerDTO
    {
        public CustomerDTO()
        {
            Name = String.Empty;
            Surname = String.Empty;
            Email = String.Empty;
            Password = String.Empty;
        }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
