using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RowadSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnInOrderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_ApplicationUserId",
                table: "RefreshTokens");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Orders",
                newName: "Status");

            migrationBuilder.AddColumn<string>(
                name: "CouponCode",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmount",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethod",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PaymentTransactionId",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1,
                column: "ClaimValue",
                value: "User.View");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2,
                column: "ClaimValue",
                value: "User.Create");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3,
                column: "ClaimValue",
                value: "User.Edit");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 4,
                column: "ClaimValue",
                value: "User.Delete");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5,
                column: "ClaimValue",
                value: "Product.View");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6,
                column: "ClaimValue",
                value: "Product.Create");

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 7, "Permissions", "Product.Edit", "0abbc730-72e4-4a94-b148-4a209a7a42df" },
                    { 8, "Permissions", "Product.Delete", "0abbc730-72e4-4a94-b148-4a209a7a42df" },
                    { 9, "Permissions", "Invoice.View", "0abbc730-72e4-4a94-b148-4a209a7a42df" },
                    { 10, "Permissions", "Invoice.Create", "0abbc730-72e4-4a94-b148-4a209a7a42df" },
                    { 11, "Permissions", "Invoice.Edit", "0abbc730-72e4-4a94-b148-4a209a7a42df" },
                    { 12, "Permissions", "Invoice.Delete", "0abbc730-72e4-4a94-b148-4a209a7a42df" },
                    { 13, "Permissions", "Role.View", "0abbc730-72e4-4a94-b148-4a209a7a42df" },
                    { 14, "Permissions", "Role.Create", "0abbc730-72e4-4a94-b148-4a209a7a42df" },
                    { 15, "Permissions", "Role.Edit", "0abbc730-72e4-4a94-b148-4a209a7a42df" },
                    { 16, "Permissions", "Role.Delete", "0abbc730-72e4-4a94-b148-4a209a7a42df" },
                    { 17, "Permissions", "Report.View", "0abbc730-72e4-4a94-b148-4a209a7a42df" },
                    { 18, "Permissions", "Report.Create", "0abbc730-72e4-4a94-b148-4a209a7a42df" },
                    { 19, "Permissions", "Report.Edit", "0abbc730-72e4-4a94-b148-4a209a7a42df" },
                    { 20, "Permissions", "Report.Delete", "0abbc730-72e4-4a94-b148-4a209a7a42df" },
                    { 21, "Permissions", "Brand.View", "0abbc730-72e4-4a94-b148-4a209a7a42df" },
                    { 22, "Permissions", "Brand.Create", "0abbc730-72e4-4a94-b148-4a209a7a42df" },
                    { 23, "Permissions", "Brand.Edit", "0abbc730-72e4-4a94-b148-4a209a7a42df" },
                    { 24, "Permissions", "Brand.Delete", "0abbc730-72e4-4a94-b148-4a209a7a42df" },
                    { 25, "Permissions", "Category.View", "0abbc730-72e4-4a94-b148-4a209a7a42df" },
                    { 26, "Permissions", "Category.Create", "0abbc730-72e4-4a94-b148-4a209a7a42df" },
                    { 27, "Permissions", "Category.Edit", "0abbc730-72e4-4a94-b148-4a209a7a42df" },
                    { 28, "Permissions", "Category.Delete", "0abbc730-72e4-4a94-b148-4a209a7a42df" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2fe9d316-b5b7-40e9-9c54-23114c21145c",
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 16, 14, 45, 47, 276, DateTimeKind.Utc).AddTicks(4954), "AQAAAAIAAYagAAAAECKorBCGTy4svXO4kL9cOtmBHIh1V3p3fI0TUo0Ipido1Ep7ZeADP4imJmVfrJZCSA==" });

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

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DropColumn(
                name: "CouponCode",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PaymentTransactionId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Orders",
                newName: "status");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1,
                column: "ClaimValue",
                value: "User:Read");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2,
                column: "ClaimValue",
                value: "User:Add");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3,
                column: "ClaimValue",
                value: "User:Update");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 4,
                column: "ClaimValue",
                value: "Role:Read");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5,
                column: "ClaimValue",
                value: "Role:Add");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6,
                column: "ClaimValue",
                value: "Role:Update");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2fe9d316-b5b7-40e9-9c54-23114c21145c",
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 11, 9, 53, 28, 346, DateTimeKind.Utc).AddTicks(7294), "AQAAAAIAAYagAAAAEP2ajHQiiZO317JrMiXmenZvIdTOFvWQyLqHpSnORwEWbEP9KflSZ/jGrAKd2h8CTw==" });

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
