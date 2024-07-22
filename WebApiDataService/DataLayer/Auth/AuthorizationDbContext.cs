using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using WebApiDataService.DataLayer.Auth.Models;

namespace WebApiDataService.DataLayer.Auth
{
    public class AuthorizationDbContext : DbContext
    {
        internal DbSet<ApiKey> ApiKeys { get; set; }
        internal DbSet<Client> Clients { get; set; }

        public AuthorizationDbContext(DbContextOptions<AuthorizationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<ApiKey>().HasIndex(x => x.Id).IsUnique();

            modelBuilder
                .Entity<ApiKey>()
                .Property(b => b.ApiKeyValue)
                .IsRequired();


            modelBuilder
                .Entity<ApiKey>()
                .HasIndex(b => b.ApiKeyValue).IsUnique();


            modelBuilder
                .Entity<ApiKey>()
                .Property(b => b.PaymentPlan)
                .IsRequired();

            modelBuilder
                .Entity<ApiKey>()
                .Property(b => b.CreatedAt)
                .IsRequired();

            modelBuilder
                .Entity<ApiKey>()
                .Property(b => b.State)
                .IsRequired();


            modelBuilder
                .Entity<ApiKey>()
                .HasOne<Client>()
                .WithOne(x => x.ApiKey)
                .HasForeignKey<ApiKey>(x => x.ClientId).OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder
                .Entity<Client>().HasIndex(x => x.Id).IsUnique();

            modelBuilder
                .Entity<Client>()
                .Property(b => b.Name)
                .IsRequired();
        }
    }
}
