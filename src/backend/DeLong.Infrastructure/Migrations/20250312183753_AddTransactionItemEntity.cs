using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeLong.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionItemEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionItem_Products_ProductId",
                table: "TransactionItem");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionItem_Transactions_TransactionId",
                table: "TransactionItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactionItem",
                table: "TransactionItem");

            migrationBuilder.RenameTable(
                name: "TransactionItem",
                newName: "TransactionItems");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionItem_TransactionId",
                table: "TransactionItems",
                newName: "IX_TransactionItems_TransactionId");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionItem_ProductId",
                table: "TransactionItems",
                newName: "IX_TransactionItems_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactionItems",
                table: "TransactionItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionItems_Products_ProductId",
                table: "TransactionItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionItems_Transactions_TransactionId",
                table: "TransactionItems",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionItems_Products_ProductId",
                table: "TransactionItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionItems_Transactions_TransactionId",
                table: "TransactionItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactionItems",
                table: "TransactionItems");

            migrationBuilder.RenameTable(
                name: "TransactionItems",
                newName: "TransactionItem");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionItems_TransactionId",
                table: "TransactionItem",
                newName: "IX_TransactionItem_TransactionId");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionItems_ProductId",
                table: "TransactionItem",
                newName: "IX_TransactionItem_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactionItem",
                table: "TransactionItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionItem_Products_ProductId",
                table: "TransactionItem",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionItem_Transactions_TransactionId",
                table: "TransactionItem",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
