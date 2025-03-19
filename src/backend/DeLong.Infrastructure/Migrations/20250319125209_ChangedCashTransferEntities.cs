using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeLong.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangedCashTransferEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DebtAmount",
                table: "CashTransfers");

            migrationBuilder.DropColumn(
                name: "UsdBalance",
                table: "CashTransfers");

            migrationBuilder.DropColumn(
                name: "UzpBalance",
                table: "CashTransfers");

            migrationBuilder.RenameColumn(
                name: "UzsBalance",
                table: "CashTransfers",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "TransferType",
                table: "CashTransfers",
                newName: "To");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "CashTransfers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "From",
                table: "CashTransfers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "CashTransfers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "TransferDate",
                table: "CashTransfers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "CashTransfers");

            migrationBuilder.DropColumn(
                name: "From",
                table: "CashTransfers");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "CashTransfers");

            migrationBuilder.DropColumn(
                name: "TransferDate",
                table: "CashTransfers");

            migrationBuilder.RenameColumn(
                name: "To",
                table: "CashTransfers",
                newName: "TransferType");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "CashTransfers",
                newName: "UzsBalance");

            migrationBuilder.AddColumn<decimal>(
                name: "DebtAmount",
                table: "CashTransfers",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UsdBalance",
                table: "CashTransfers",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UzpBalance",
                table: "CashTransfers",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
