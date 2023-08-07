using KaspelTestTask.Core.Domain;
using KaspelTestTask.Domain;
using Microsoft.EntityFrameworkCore;

namespace KaspelTestTask.Application.Interfaces;

public interface IKaspelTestTaskDbContext
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Stock> Stock { get; set; }

    public Task SaveChangesAsync();
}

