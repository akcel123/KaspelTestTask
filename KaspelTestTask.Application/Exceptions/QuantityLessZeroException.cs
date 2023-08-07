namespace KaspelTestTask.Application.Exceptions;

public class QuantityLessZeroException: Exception
{
	public QuantityLessZeroException() : base("Количество книг при заказе не может быть меньше или равно 0")
	{ }
}

