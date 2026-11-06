using FTELMarket.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace FTELMarket.Infrastructure;

public class FtelMarketDbContext : DbContext
{
    public DbSet<Category> Category { get; set;}

    public DbSet<Product> Product { get; set;}

    public DbSet<Customer> Customer {get;set;}

    public FtelMarketDbContext(DbContextOptions<FtelMarketDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FtelMarketDbContext).Assembly);
    }
}
