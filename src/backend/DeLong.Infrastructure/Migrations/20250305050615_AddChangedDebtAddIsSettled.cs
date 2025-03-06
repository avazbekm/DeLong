using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeLong.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddChangedDebtAddIsSettled : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSettled",
                table: "Debts",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSettled",
                table: "Debts");
        }
    }
}
