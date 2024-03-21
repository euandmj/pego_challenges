using Inventory.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Inventory;

public static class Extensions
{

	public static void ApplyMigrations(this IApplicationBuilder app)
	{
		using var scope = app.ApplicationServices.CreateScope();

		using var dbContext = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();

		//dbContext.Database.Migrate();
	}
}
