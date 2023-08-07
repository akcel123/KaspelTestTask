namespace KaspelTestTask.Application.Exceptions;

public class DtoIsNotValidException: Exception
{
	public DtoIsNotValidException(string message): base(message)
	{ }
}

