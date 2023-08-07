namespace KaspelTestTask.API.Models.Order;

public class AddBookToOrder
{
    public Guid BookId { get; set; }
    public int Quantity { get; set; }
}

