using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineShop.Secondary.Adapters.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                                         "Category",
                                         table => new
                                                  {
                                                      Id = table.Column<Guid>(nullable: false,
                                                                              defaultValueSql: "NEWSEQUENTIALID()"),
                                                      DeletedOn = table.Column<DateTime>(nullable: true),
                                                      Version = table.Column<byte[]>(rowVersion: true, nullable: true),
                                                      Name = table.Column<string>(nullable: true)
                                                  },
                                         constraints: table => { table.PrimaryKey("PK_Category", x => x.Id); });

            migrationBuilder.CreateTable(
                                         "User",
                                         table => new
                                                  {
                                                      Id = table.Column<Guid>(nullable: false,
                                                                              defaultValueSql: "NEWSEQUENTIALID()"),
                                                      DeletedOn = table.Column<DateTime>(nullable: true),
                                                      Version = table.Column<byte[]>(rowVersion: true, nullable: true),
                                                      Username = table.Column<string>(),
                                                      Email = table.Column<string>(),
                                                      FirstName = table.Column<string>(nullable: true),
                                                      LastName = table.Column<string>(nullable: true),
                                                      Password = table.Column<string>(),
                                                      Salt = table.Column<byte[]>()
                                                  },
                                         constraints: table => { table.PrimaryKey("PK_User", x => x.Id); });

            migrationBuilder.CreateTable(
                                         "UserCart",
                                         table => new
                                                  {
                                                      Id = table.Column<Guid>(nullable: false,
                                                                              defaultValueSql: "NEWSEQUENTIALID()"),
                                                      DeletedOn = table.Column<DateTime>(nullable: true),
                                                      Version = table.Column<byte[]>(rowVersion: true, nullable: true),
                                                      Total = table.Column<decimal>("decimal(18,4)"),
                                                      UserId = table.Column<Guid>()
                                                  },
                                         constraints: table =>
                                                      {
                                                          table.PrimaryKey("PK_UserCart", x => x.Id);
                                                          table.ForeignKey(
                                                                           "FK_UserCart_User_UserId",
                                                                           x => x.UserId,
                                                                           "User",
                                                                           "Id",
                                                                           onDelete: ReferentialAction.Cascade);
                                                      });

            migrationBuilder.CreateTable(
                                         "CartItem",
                                         table => new
                                                  {
                                                      Id = table.Column<Guid>(nullable: false,
                                                                              defaultValueSql: "NEWSEQUENTIALID()"),
                                                      DeletedOn = table.Column<DateTime>(nullable: true),
                                                      Version = table.Column<byte[]>(rowVersion: true, nullable: true),
                                                      Quantity = table.Column<decimal>("decimal(18,2)"),
                                                      Price = table.Column<decimal>("decimal(18,4)"),
                                                      UserCartId = table.Column<Guid>(),
                                                      ProductId = table.Column<Guid>()
                                                  },
                                         constraints: table =>
                                                      {
                                                          table.PrimaryKey("PK_CartItem", x => x.Id);
                                                          table.ForeignKey(
                                                                           "FK_CartItem_UserCart_UserCartId",
                                                                           x => x.UserCartId,
                                                                           "UserCart",
                                                                           "Id",
                                                                           onDelete: ReferentialAction.Cascade);
                                                      });

            migrationBuilder.CreateTable(
                                         "Product",
                                         table => new
                                                  {
                                                      Id = table.Column<Guid>(nullable: false,
                                                                              defaultValueSql: "NEWSEQUENTIALID()"),
                                                      DeletedOn = table.Column<DateTime>(nullable: true),
                                                      Version = table.Column<byte[]>(rowVersion: true, nullable: true),
                                                      Name = table.Column<string>(nullable: true),
                                                      Price = table.Column<decimal>("decimal(18,4)", nullable: true),
                                                      Quantity = table.Column<decimal>("decimal(18,2)", nullable: true),
                                                      Code = table.Column<string>(nullable: true),
                                                      CategoryId = table.Column<Guid>(),
                                                      CartItemId = table.Column<Guid>(nullable: true)
                                                  },
                                         constraints: table =>
                                                      {
                                                          table.PrimaryKey("PK_Product", x => x.Id);
                                                          table.ForeignKey(
                                                                           "FK_Product_CartItem_CartItemId",
                                                                           x => x.CartItemId,
                                                                           "CartItem",
                                                                           "Id",
                                                                           onDelete: ReferentialAction.Restrict);
                                                          table.ForeignKey(
                                                                           "FK_Product_Category_CategoryId",
                                                                           x => x.CategoryId,
                                                                           "Category",
                                                                           "Id",
                                                                           onDelete: ReferentialAction.Cascade);
                                                      });

            migrationBuilder.CreateIndex(
                                         "IX_CartItem_UserCartId",
                                         "CartItem",
                                         "UserCartId");

            migrationBuilder.CreateIndex(
                                         "IX_Product_CartItemId",
                                         "Product",
                                         "CartItemId",
                                         unique: true,
                                         filter: "[CartItemId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                                         "IX_Product_CategoryId",
                                         "Product",
                                         "CategoryId");

            migrationBuilder.CreateIndex(
                                         "IX_UserCart_UserId",
                                         "UserCart",
                                         "UserId",
                                         unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                                       "Product");

            migrationBuilder.DropTable(
                                       "CartItem");

            migrationBuilder.DropTable(
                                       "Category");

            migrationBuilder.DropTable(
                                       "UserCart");

            migrationBuilder.DropTable(
                                       "User");
        }
    }
}