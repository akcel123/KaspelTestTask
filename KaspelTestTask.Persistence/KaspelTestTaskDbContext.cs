using KaspelTestTask.Application.Interfaces;
using KaspelTestTask.Core.Domain;
using KaspelTestTask.Domain;
using KaspelTestTask.Persistence.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;

namespace KaspelTestTask.Persistence;

public class KaspelTestTaskDbContext: DbContext, IKaspelTestTaskDbContext
{
	public DbSet<Book> Books { get; set; }
	public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Stock> Stock { get; set; }

    public KaspelTestTaskDbContext(DbContextOptions<KaspelTestTaskDbContext> options): base(options)
	{}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BookConfiguration());
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.ApplyConfiguration(new OrderItemConfiguration());
        modelBuilder.ApplyConfiguration(new StockConfiguration());
        base.OnModelCreating(modelBuilder);
    }

    public async Task SaveChangesAsync()
    {
        await base.SaveChangesAsync();
    }
}

