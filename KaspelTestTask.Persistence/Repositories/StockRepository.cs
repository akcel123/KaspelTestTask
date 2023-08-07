using System;
using KaspelTestTask.Application.Exceptions;
using KaspelTestTask.Application.Interfaces;
using KaspelTestTask.Domain;
using Microsoft.EntityFrameworkCore;

namespace KaspelTestTask.Persistence.Repositories;

public class StockRepository: IStockRepository
{
    readonly IKaspelTestTaskDbContext _dbContext;

    public StockRepository(IKaspelTestTaskDbContext dbContext)
        => _dbContext = dbContext;

    public async Task DecreaserNumberOfBookByIdAsync(Guid id, int decrease)
    {
        var entity = await _dbContext.Stock.FirstOrDefaultAsync(opt => opt.Book.Id == id) ?? throw new ContentNotFoundException();
        entity.Quantity -= decrease;
        await _dbContext.SaveChangesAsync();
    }

    public async Task IncreaserNumberOfBookByIdAsync(Guid id, int increase)
    {
        var entity = await _dbContext.Stock.FirstOrDefaultAsync(opt => opt.Book.Id == id) ?? throw new ContentNotFoundException();
        entity.Quantity += increase;
        await _dbContext.SaveChangesAsync();
    }

    public async Task<int> GetNumberOfBookByIdAsync(Guid id)
    {
        var entity = await _dbContext.Stock.FirstOrDefaultAsync(opt => opt.Book.Id == id) ?? throw new ContentNotFoundException();
        return entity.Quantity;
    }
}

