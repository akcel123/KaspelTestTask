namespace KaspelTestTask.Application.Exceptions;

public class FewBooksInStockException: Exception
{
	public FewBooksInStockException(string message): base(message)
	{ }
}

