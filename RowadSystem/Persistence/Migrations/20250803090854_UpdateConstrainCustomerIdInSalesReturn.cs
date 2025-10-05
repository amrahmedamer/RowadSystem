using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RowadSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateConstrainCustomerIdInSalesReturn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_ApplicationUserId",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesReturns_Customers_CustomerId",
                table: "SalesReturns");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "SalesReturns",
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
                name: "FK_RefreshTokens_AspNetUsers_ApplicationUserId",
                table: "RefreshTokens",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesReturns_Customers_CustomerId",
                table: "SalesReturns",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_ApplicationUserId",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesReturns_Customers_CustomerId",
                table: "SalesReturns");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "SalesReturns",
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
                values: new object[] { new DateTime(2025, 8, 3, 8, 17, 55, 413, DateTimeKind.Utc).AddTicks(1309), "AQAAAAIAAYagAAAAECmqaOBZucHft6e3oHoXA7OSiC0g9MWqrdRQ2ZrmU8GIypD3D6trD4A3Al93BB0UAA==" });

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_ApplicationUserId",
                table: "RefreshTokens",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesReturns_Customers_CustomerId",
                table: "SalesReturns",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
