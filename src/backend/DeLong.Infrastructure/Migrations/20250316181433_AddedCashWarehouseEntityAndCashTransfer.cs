using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DeLong.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedCashWarehouseEntityAndCashTransfer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CashTransfer_CashRegisters_CashRegisterId",
                table: "CashTransfer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CashTransfer",
                table: "CashTransfer");

            migrationBuilder.RenameTable(
                name: "CashTransfer",
                newName: "CashTransfers");

            migrationBuilder.RenameIndex(
                name: "IX_CashTransfer_CashRegisterId",
                table: "CashTransfers",
                newName: "IX_CashTransfers_CashRegisterId");

            migrationBuilder.AddColumn<long>(
                name: "CashWarehouseId",
                table: "CashRegisters",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CashTransfers",
                table: "CashTransfers",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CashWarehouses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UzsBalance = table.Column<decimal>(type: "numeric", nullable: false),
                    UzpBalance = table.Column<decimal>(type: "numeric", nullable: false),
                    UsdBalance = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashWarehouses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CashRegisters_CashWarehouseId",
                table: "CashRegisters",
                column: "CashWarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_CashRegisters_CashWarehouses_CashWarehouseId",
                table: "CashRegisters",
                column: "CashWarehouseId",
                principalTable: "CashWarehouses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CashTransfers_CashRegisters_CashRegisterId",
                table: "CashTransfers",
                column: "CashRegisterId",
                principalTable: "CashRegisters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CashRegisters_CashWarehouses_CashWarehouseId",
                table: "CashRegisters");

            migrationBuilder.DropForeignKey(
                name: "FK_CashTransfers_CashRegisters_CashRegisterId",
                table: "CashTransfers");

            migrationBuilder.DropTable(
                name: "CashWarehouses");

            migrationBuilder.DropIndex(
                name: "IX_CashRegisters_CashWarehouseId",
                table: "CashRegisters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CashTransfers",
                table: "CashTransfers");

            migrationBuilder.DropColumn(
                name: "CashWarehouseId",
                table: "CashRegisters");

            migrationBuilder.RenameTable(
                name: "CashTransfers",
                newName: "CashTransfer");

            migrationBuilder.RenameIndex(
                name: "IX_CashTransfers_CashRegisterId",
                table: "CashTransfer",
                newName: "IX_CashTransfer_CashRegisterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CashTransfer",
                table: "CashTransfer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CashTransfer_CashRegisters_CashRegisterId",
                table: "CashTransfer",
                column: "CashRegisterId",
                principalTable: "CashRegisters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
