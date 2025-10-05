using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RowadSystem.Migrations
{
    /// <inheritdoc />
    public partial class RemoveGovernrateIdFromOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_ApplicationUserId",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "GovernorateId",
                table: "Orders");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2fe9d316-b5b7-40e9-9c54-23114c21145c",
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 3, 7, 29, 21, 675, DateTimeKind.Utc).AddTicks(6935), "AQAAAAIAAYagAAAAEJHx5xtaDCiYbrmJHYQQ2y+kHnd+OHOrhIhHug69MlZUH1US/nkgWA4tGcEpsiDEVw==" });

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

            migrationBuilder.AddColumn<int>(
                name: "GovernorateId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2fe9d316-b5b7-40e9-9c54-23114c21145c",
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 3, 7, 23, 20, 291, DateTimeKind.Utc).AddTicks(4755), "AQAAAAIAAYagAAAAEFDpS6IwaSty0nT2d9m5pe4+grVJV7N0ZZOuPCladyALDqQo4COlerD4Lgjqm8QdDg==" });

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
