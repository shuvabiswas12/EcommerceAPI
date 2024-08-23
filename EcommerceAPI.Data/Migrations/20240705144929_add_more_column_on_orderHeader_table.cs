using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommerceAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class add_more_column_on_orderHeader_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShippingAddress",
                table: "OrderHeaders",
                newName: "RoadNumber");

            migrationBuilder.RenameColumn(
                name: "OrderTotal",
                table: "OrderHeaders",
                newName: "OrderAmount");

            migrationBuilder.AddColumn<string>(
                name: "AddressLine1",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressLine2",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AlternatePhone",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HouseName",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LandMark",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentIntentId",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Province",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressLine1",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "AddressLine2",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "AlternatePhone",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "HouseName",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "LandMark",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "PaymentIntentId",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "Province",
                table: "OrderHeaders");

            migrationBuilder.RenameColumn(
                name: "RoadNumber",
                table: "OrderHeaders",
                newName: "ShippingAddress");

            migrationBuilder.RenameColumn(
                name: "OrderAmount",
                table: "OrderHeaders",
                newName: "OrderTotal");
        }
    }
}
