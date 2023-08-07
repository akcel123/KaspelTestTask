using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using KaspelTestTask.Application.Interfaces;
using KaspelTestTask.Persistence.Repositories;

namespace KaspelTestTask.Persistence;


public static class DependencyInjection
{
	public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString("database");
		services.AddDbContext<KaspelTestTaskDbContext>(options =>
			options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 1, 0)))
		);

        services.AddScoped<IKaspelTestTaskDbContext>(provider => provider.GetService<KaspelTestTaskDbContext>()!);

		//repositories
		services.AddScoped<IBookRepository, BookRepository>();
		services.AddScoped<IOrderRepository, OrderRepository>();
		services.AddScoped<IOrderItemRepository, OrderItemRepository>();
		services.AddScoped<IStockRepository, StockRepository>();

		return services;
	}
}

