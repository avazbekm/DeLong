using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DeLong.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCashWarehouseEntityAndCashTransfer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ClosedAt",
                table: "CashRegisters",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DebtAmount",
                table: "CashRegisters",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "OpenedAt",
                table: "CashRegisters",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "CashRegisters",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "CashTransfer",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CashRegisterId = table.Column<long>(type: "bigint", nullable: false),
                    UzsBalance = table.Column<decimal>(type: "numeric", nullable: false),
                    UzpBalance = table.Column<decimal>(type: "numeric", nullable: false),
                    UsdBalance = table.Column<decimal>(type: "numeric", nullable: false),
                    DebtAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    TransferType = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashTransfer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashTransfer_CashRegisters_CashRegisterId",
                        column: x => x.CashRegisterId,
                        principalTable: "CashRegisters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CashRegisters_UserId",
                table: "CashRegisters",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CashTransfer_CashRegisterId",
                table: "CashTransfer",
                column: "CashRegisterId");

            migrationBuilder.AddForeignKey(
                name: "FK_CashRegisters_Users_UserId",
                table: "CashRegisters",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CashRegisters_Users_UserId",
                table: "CashRegisters");

            migrationBuilder.DropTable(
                name: "CashTransfer");

            migrationBuilder.DropIndex(
                name: "IX_CashRegisters_UserId",
                table: "CashRegisters");

            migrationBuilder.DropColumn(
                name: "ClosedAt",
                table: "CashRegisters");

            migrationBuilder.DropColumn(
                name: "DebtAmount",
                table: "CashRegisters");

            migrationBuilder.DropColumn(
                name: "OpenedAt",
                table: "CashRegisters");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CashRegisters");
        }
    }
}
