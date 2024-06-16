using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingBasketAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class added_data_seeding_for_image_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Images",
                keyColumns: new[] { "ImageUrl", "ProductId" },
                keyValues: new object[] { "https://technave.com/data/files/mall/article/202103171440419975.jpg", "" });

            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "ImageUrl", "ProductId" },
                values: new object[] { "https://technave.com/data/files/mall/article/202103171440419975.jpg", "d72e6d76-ffb8-4b6a-b2b7-5945f84992d9" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Images",
                keyColumns: new[] { "ImageUrl", "ProductId" },
                keyValues: new object[] { "https://technave.com/data/files/mall/article/202103171440419975.jpg", "d72e6d76-ffb8-4b6a-b2b7-5945f84992d9" });

            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "ImageUrl", "ProductId" },
                values: new object[] { "https://technave.com/data/files/mall/article/202103171440419975.jpg", "" });
        }
    }
}
