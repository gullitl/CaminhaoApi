using CaminhaoApi.Domain.CaminhaoAggregate;
using Microsoft.EntityFrameworkCore;

namespace CaminhaoApi.Infrastructure.Database.Contexts
{
    public class DatabaseContext : DbContext
    {

        public virtual DbSet<Caminhao> Caminhoes { get; set; }

        public DatabaseContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();

            if (Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
                Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) => base.OnConfiguring(options);
        protected override void OnModelCreating(ModelBuilder modelBuilder) => base.OnModelCreating(modelBuilder);

    }
}
