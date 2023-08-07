namespace KaspelTestTask.Application.Interfaces;

public interface IStockRepository
{
	public Task IncreaserNumberOfBookByIdAsync(Guid id, int increase);
	public Task DecreaserNumberOfBookByIdAsync(Guid id, int decrease);
	public Task<int> GetNumberOfBookByIdAsync(Guid id);
}

