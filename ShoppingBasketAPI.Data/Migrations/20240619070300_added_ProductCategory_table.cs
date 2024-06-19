using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingBasketAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class added_ProductCategory_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Images",
                keyColumns: new[] { "ImageUrl", "ProductId" },
                keyValues: new object[] { "https://technave.com/data/files/mall/article/202103171440419975.jpg", "00e078bd-165b-4a89-aa4f-034acdd0530e" });

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: "00e078bd-165b-4a89-aa4f-034acdd0530e");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "ProductCategory",
                columns: table => new
                {
                    ProductId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CategoryId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategory", x => new { x.ProductId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_ProductCategory_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductCategory_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategory_CategoryId",
                table: "ProductCategory",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategory_ProductId",
                table: "ProductCategory",
                column: "ProductId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductCategory");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Products");

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "Name", "Price" },
                values: new object[] { "00e078bd-165b-4a89-aa4f-034acdd0530e", "***Galaxy A52 is rated as IP67. Based on test conditions for submersion in up to 1 meter of freshwater for up to 30 minutes. Not advised for beach, pool use and soapy water. In case you spill liquids containing sugar on the phone, please rinse the device in clean, stagnant water while clicking keys. Safe against low water pressure only. High water pressure such as running tap water or shower may damage the device.", "Samsung Galaxy A52 4g", 25000m });

            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "ImageUrl", "ProductId" },
                values: new object[] { "https://technave.com/data/files/mall/article/202103171440419975.jpg", "00e078bd-165b-4a89-aa4f-034acdd0530e" });
        }
    }
}
