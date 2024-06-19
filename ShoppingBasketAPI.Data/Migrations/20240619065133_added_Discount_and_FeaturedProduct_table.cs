using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingBasketAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class added_Discount_and_FeaturedProduct_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Images",
                keyColumns: new[] { "ImageUrl", "ProductId" },
                keyValues: new object[] { "https://technave.com/data/files/mall/article/202103171440419975.jpg", "d72e6d76-ffb8-4b6a-b2b7-5945f84992d9" });

            migrationBuilder.CreateTable(
                name: "Discounts",
                columns: table => new
                {
                    ProductId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DiscountRate = table.Column<double>(type: "float", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Discounts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeaturedProducts",
                columns: table => new
                {
                    ProductId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeaturedProducts", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_FeaturedProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "Name", "Price" },
                values: new object[] { "00e078bd-165b-4a89-aa4f-034acdd0530e", "***Galaxy A52 is rated as IP67. Based on test conditions for submersion in up to 1 meter of freshwater for up to 30 minutes. Not advised for beach, pool use and soapy water. In case you spill liquids containing sugar on the phone, please rinse the device in clean, stagnant water while clicking keys. Safe against low water pressure only. High water pressure such as running tap water or shower may damage the device.", "Samsung Galaxy A52 4g", 25000m });

            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "ImageUrl", "ProductId" },
                values: new object[] { "https://technave.com/data/files/mall/article/202103171440419975.jpg", "00e078bd-165b-4a89-aa4f-034acdd0530e" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.DropTable(
                name: "FeaturedProducts");

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumns: new[] { "ImageUrl", "ProductId" },
                keyValues: new object[] { "https://technave.com/data/files/mall/article/202103171440419975.jpg", "00e078bd-165b-4a89-aa4f-034acdd0530e" });

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: "00e078bd-165b-4a89-aa4f-034acdd0530e");

            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "ImageUrl", "ProductId" },
                values: new object[] { "https://technave.com/data/files/mall/article/202103171440419975.jpg", "d72e6d76-ffb8-4b6a-b2b7-5945f84992d9" });
        }
    }
}
