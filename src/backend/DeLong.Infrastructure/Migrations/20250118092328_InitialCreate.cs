using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeLong.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CashRegisters_Warehouses_WarehouseId1",
                table: "CashRegisters");

            migrationBuilder.DropIndex(
                name: "IX_CashRegisters_WarehouseId1",
                table: "CashRegisters");

            migrationBuilder.DropColumn(
                name: "WarehouseId1",
                table: "CashRegisters");

            migrationBuilder.AlterColumn<long>(
                name: "WarehouseId",
                table: "CashRegisters",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_CashRegisters_WarehouseId",
                table: "CashRegisters",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_CashRegisters_Warehouses_WarehouseId",
                table: "CashRegisters",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CashRegisters_Warehouses_WarehouseId",
                table: "CashRegisters");

            migrationBuilder.DropIndex(
                name: "IX_CashRegisters_WarehouseId",
                table: "CashRegisters");

            migrationBuilder.AlterColumn<int>(
                name: "WarehouseId",
                table: "CashRegisters",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "WarehouseId1",
                table: "CashRegisters",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_CashRegisters_WarehouseId1",
                table: "CashRegisters",
                column: "WarehouseId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CashRegisters_Warehouses_WarehouseId1",
                table: "CashRegisters",
                column: "WarehouseId1",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
