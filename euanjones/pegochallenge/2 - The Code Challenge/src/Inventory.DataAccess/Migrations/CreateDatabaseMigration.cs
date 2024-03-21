using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Inventory.DataAccess.Migrations;

public partial class CreateDatabaseMigration : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable(
			name: "Books",
			columns: table => new
			{
				Id = table.Column<long>(type: "bigint", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
				AuthorName = table.Column<string>(type: "text", nullable: false),
				Title = table.Column<string>(type: "text", nullable: false),
				PublishDate = table.Column<DateOnly>(type: "date", nullable: false)
			},
			constraints: table => table.PrimaryKey("PK_Books", x => x.Id));
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable("Books");
	}
}
