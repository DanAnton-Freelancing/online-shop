using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineShop.Secondary.Adapters.Migrations
{
    public partial class ModifyQuantityFieldForProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Product",
                newName: "AvailableQuantity");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AvailableQuantity",
                table: "Product",
                newName: "Quantity");
        }
    }
}
