using Microsoft.EntityFrameworkCore;
using PaymentGateway.Data.Entities;

namespace PaymentGateway.Data
{
    /// <summary>
    /// DB Context for Gateway 
    /// </summary>
    public class GatewayDbContext : DbContext
    {

        public GatewayDbContext() : base()
        {

        }

        public GatewayDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ensure that API Keys are Unique
            modelBuilder.Entity<Merchant>()
                .HasIndex(u => u.ApiKey)
                .IsUnique();
            // seed the database
            modelBuilder.Seed();
        }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Merchant> Merchants { get; set; }
    }
}