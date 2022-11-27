using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineShop.Secondary.Adapters.Migrations
{
    public partial class RemoveDeletedOnField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "UserCart");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "User");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "CartItem");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "UserCart",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Product",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Category",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "CartItem",
                nullable: true);
        }
    }
}
