using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingBasketAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class added_data_seeding_for_products_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: "fa224d3a-2eb7-4045-9861-efd1e9882568");

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "Name", "Price" },
                values: new object[] { "d72e6d76-ffb8-4b6a-b2b7-5945f84992d9", "***Galaxy A52 is rated as IP67. Based on test conditions for submersion in up to 1 meter of freshwater for up to 30 minutes. Not advised for beach, pool use and soapy water. In case you spill liquids containing sugar on the phone, please rinse the device in clean, stagnant water while clicking keys. Safe against low water pressure only. High water pressure such as running tap water or shower may damage the device.", "Samsung Galaxy A52 4g", 25000m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: "d72e6d76-ffb8-4b6a-b2b7-5945f84992d9");

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "Name", "Price" },
                values: new object[] { "fa224d3a-2eb7-4045-9861-efd1e9882568", "***Galaxy A52 is rated as IP67. Based on test conditions for submersion in up to 1 meter of freshwater for up to 30 minutes. Not advised for beach, pool use and soapy water. In case you spill liquids containing sugar on the phone, please rinse the device in clean, stagnant water while clicking keys. Safe against low water pressure only. High water pressure such as running tap water or shower may damage the device.", "Samsung Galaxy A52 4g", 25000m });
        }
    }
}
