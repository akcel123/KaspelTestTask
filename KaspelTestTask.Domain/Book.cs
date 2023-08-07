using KaspelTestTask.Domain;

namespace KaspelTestTask.Core.Domain;

public class Book
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public DateTime ReleaseDate { get; set; }
    public decimal Price { get; set; }

    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}