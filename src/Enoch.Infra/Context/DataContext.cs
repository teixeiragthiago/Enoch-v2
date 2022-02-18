using Enoch.Domain.Services.User.Entities;
using Microsoft.EntityFrameworkCore;

namespace Enoch.Infra.Context
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DbSet<UserEntity> User { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder
                .UseInMemoryDatabase(databaseName: "enoch");
        }
    }
}
