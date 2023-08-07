namespace KaspelTestTask.Core.Domain;

public class OrderItem
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public int Quantity { get; set; }
    public Guid OrderId { get; set; }
}

