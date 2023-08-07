using KaspelTestTask.Core.Domain;

namespace KaspelTestTask.Domain;

public class Stock
{
    public int Quantity { get; set; }

    public Guid BookId { get; set; }
    public Book Book { get; set; }
}

