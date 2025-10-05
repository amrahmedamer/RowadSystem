using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RowadSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddCashierHandoverTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_ApplicationUserId",
                table: "RefreshTokens");

            migrationBuilder.CreateTable(
                name: "CashierHandovers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CashierId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AmountInDrawer = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalSalesAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalReturnAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPurchaseAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPurchaseReturnAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Difference = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HandoverDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HandoverStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HandoverEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsLateHandover = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashierHandovers", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2fe9d316-b5b7-40e9-9c54-23114c21145c",
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 10, 18, 31, 38, 115, DateTimeKind.Utc).AddTicks(1006), "AQAAAAIAAYagAAAAEDaqPGTKZyScYKh6dhnVQpJox801vZE3xBHFo9jG0cFlPVKCpVvVvBY9Pa/Ea39/rA==" });

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_ApplicationUserId",
                table: "RefreshTokens",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_ApplicationUserId",
                table: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "CashierHandovers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2fe9d316-b5b7-40e9-9c54-23114c21145c",
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 7, 11, 3, 53, 343, DateTimeKind.Utc).AddTicks(2703), "AQAAAAIAAYagAAAAEI8zCccI3TuQ2ytx9IWc3hcJp/CPDduO503zWFkDyoaqBiIjfoy0XQbs8jMDtDyCgw==" });

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_ApplicationUserId",
                table: "RefreshTokens",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
