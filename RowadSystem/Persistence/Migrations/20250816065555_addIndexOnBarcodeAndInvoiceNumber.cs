using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RowadSystem.Migrations
{
    /// <inheritdoc />
    public partial class addIndexOnBarcodeAndInvoiceNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceDetails_Units_UnitId",
                table: "InvoiceDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Units_UnitId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_ApplicationUserId",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartItems_Units_UnitId",
                table: "ShoppingCartItems");

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "ShoppingCartItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Barcode",
                table: "Products",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InvoiceNumber",
                table: "Invoices",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "InvoiceDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2fe9d316-b5b7-40e9-9c54-23114c21145c",
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 8, 16, 6, 55, 54, 914, DateTimeKind.Utc).AddTicks(7346), "AQAAAAIAAYagAAAAEE+aNCBjY+U0bxO3/kHzb1Qgj38s68pCQCYeXM73ddIoTniCMGTxBpILvy6+YPo+Ng==" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_Barcode",
                table: "Products",
                column: "Barcode");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_InvoiceNumber",
                table: "Invoices",
                column: "InvoiceNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceDetails_Units_UnitId",
                table: "InvoiceDetails",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Units_UnitId",
                table: "OrderItems",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_ApplicationUserId",
                table: "RefreshTokens",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartItems_Units_UnitId",
                table: "ShoppingCartItems",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceDetails_Units_UnitId",
                table: "InvoiceDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Units_UnitId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_ApplicationUserId",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartItems_Units_UnitId",
                table: "ShoppingCartItems");

            migrationBuilder.DropIndex(
                name: "IX_Products_Barcode",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_InvoiceNumber",
                table: "Invoices");

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "ShoppingCartItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Barcode",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "OrderItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "InvoiceNumber",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "InvoiceDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2fe9d316-b5b7-40e9-9c54-23114c21145c",
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 8, 3, 9, 8, 53, 355, DateTimeKind.Utc).AddTicks(8787), "AQAAAAIAAYagAAAAECPifqZZYi8bMWnHhtx/ZhCGFjAPfnbxlEH4d94qppbiOZlPypPwlJXVl/BYH7KW6A==" });

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceDetails_Units_UnitId",
                table: "InvoiceDetails",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Units_UnitId",
                table: "OrderItems",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_ApplicationUserId",
                table: "RefreshTokens",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartItems_Units_UnitId",
                table: "ShoppingCartItems",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id");
        }
    }
}
