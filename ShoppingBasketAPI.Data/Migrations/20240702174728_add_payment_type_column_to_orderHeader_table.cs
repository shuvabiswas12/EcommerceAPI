using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingBasketAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class add_payment_type_column_to_orderHeader_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "OrderHeaders",
                newName: "OrderStatus");

            migrationBuilder.AddColumn<string>(
                name: "PaymentType",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "OrderHeaders");

            migrationBuilder.RenameColumn(
                name: "OrderStatus",
                table: "OrderHeaders",
                newName: "Status");
        }
    }
}
