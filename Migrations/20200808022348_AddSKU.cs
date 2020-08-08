using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetCoreSqlDb.Migrations
{
    public partial class AddSKU : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SKU",
                table: "Inventory",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "unitCost",
                table: "Inventory",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SKU",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "unitCost",
                table: "Inventory");
        }
    }
}
