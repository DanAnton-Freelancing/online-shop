using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineShop.Secondary.Adapters.Migrations
{
    public partial class RemoveTotalColumnFromUserCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Total",
                table: "UserCart");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "UserCart",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
