using Inventory.DataAccess.Model;
using Microsoft.EntityFrameworkCore;

namespace Inventory.DataAccess;

public sealed class InventoryDbContext : DbContext
{
	public DbSet<BookModel> Books { get; set; }

	public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
	{
	}

    public InventoryDbContext()
    {			
    }
}
