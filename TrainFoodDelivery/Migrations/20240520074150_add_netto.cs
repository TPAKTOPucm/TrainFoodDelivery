using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainFoodDelivery.Migrations
{
    public partial class add_netto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Netto",
                table: "Products",
                type: "TEXT",
                nullable: false,
                defaultValue: 1.5m);

            migrationBuilder.AddColumn<int>(
                name: "NettoType",
                table: "Products",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Netto",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "NettoType",
                table: "Products");
        }
    }
}
