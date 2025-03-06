using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeLong.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddChangedDebtPaymentAddPaymmentMethod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "DebtPayments",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "DebtPayments");
        }
    }
}
