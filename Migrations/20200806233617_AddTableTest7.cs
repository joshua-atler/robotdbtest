using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetCoreSqlDb.Migrations
{
    public partial class AddTableTest7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "Done",
                table: "Inventory");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Inventory",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PartType",
                table: "Inventory",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "PartType",
                table: "Inventory");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Inventory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Done",
                table: "Inventory",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
