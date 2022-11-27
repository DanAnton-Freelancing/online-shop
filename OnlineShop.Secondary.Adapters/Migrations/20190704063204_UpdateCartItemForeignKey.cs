using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineShop.Secondary.Adapters.Migrations
{
    public partial class UpdateCartItemForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                                            "FK_Product_CartItem_CartItemId",
                                            "Product");

            migrationBuilder.DropIndex(
                                       "IX_Product_CartItemId",
                                       "Product");

            migrationBuilder.DropColumn(
                                        "CartItemId",
                                        "Product");

            migrationBuilder.CreateIndex(
                                         "IX_CartItem_ProductId",
                                         "CartItem",
                                         "ProductId",
                                         unique: true);

            migrationBuilder.AddForeignKey(
                                           "FK_CartItem_Product_ProductId",
                                           "CartItem",
                                           "ProductId",
                                           "Product",
                                           principalColumn: "Id",
                                           onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                                            "FK_CartItem_Product_ProductId",
                                            "CartItem");

            migrationBuilder.DropIndex(
                                       "IX_CartItem_ProductId",
                                       "CartItem");

            migrationBuilder.AddColumn<Guid>(
                                             "CartItemId",
                                             "Product",
                                             nullable: true);

            migrationBuilder.CreateIndex(
                                         "IX_Product_CartItemId",
                                         "Product",
                                         "CartItemId",
                                         unique: true,
                                         filter: "[CartItemId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                                           "FK_Product_CartItem_CartItemId",
                                           "Product",
                                           "CartItemId",
                                           "CartItem",
                                           principalColumn: "Id",
                                           onDelete: ReferentialAction.Restrict);
        }
    }
}