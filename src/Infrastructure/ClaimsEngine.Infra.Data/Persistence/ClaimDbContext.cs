using ClaimsEngine.Application.Abstractions;
using ClaimsEngine.Domain.Aggregates.ClaimAggregate;
using ClaimsEngine.Infra.Data.Outbox;
using Microsoft.EntityFrameworkCore;

namespace ClaimsEngine.Infra.Data.Persistence
{
    public class ClaimDbContext : DbContext, IUnitOfWork
    {
        public ClaimDbContext(DbContextOptions<ClaimDbContext> options) : base(options)
        {
        }

        public DbSet<Claim> Claims { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClaimDbContext).Assembly);
        }
    }
}