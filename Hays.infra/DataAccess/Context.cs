using Hays.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hays.Data.DataAccess
{
    public class Context : DbContext
    {
        public DbSet<Customers> Customers { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Customers>().HasKey(x => x.Id);
            base.OnModelCreating(builder);
        }
    }
}
