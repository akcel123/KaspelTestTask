namespace KaspelTestTask.Core.Domain;

public class Order
{
    public Guid Id { get; set; }
    public DateTime OrderDate { get; set; }
    public bool IsSaved { get; set; }       

    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}

