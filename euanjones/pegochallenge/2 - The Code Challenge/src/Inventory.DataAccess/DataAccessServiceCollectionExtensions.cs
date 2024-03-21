using Inventory.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class DataAccessServiceCollectionExtensions
{
	public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
	{
		return services.AddDbContext<InventoryDbContext>(options =>
		{
			var connStr = configuration.GetConnectionString("Inventory") ?? throw new InvalidOperationException("No such connection string in section 'Inventory'");
			//options.UseNpgsql(connStr, options =>
			//{
			//	options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
			//	options.MigrationsAssembly("Inventory.DataAccess.Migrations");
			//});
			options.UseInMemoryDatabase("inventoryDb");
		});		
	}
}