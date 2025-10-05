using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RowadSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Cities_CityId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Governorates_GovernorateId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Streets_StreetId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Cities_CityId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Governorates_GovernorateId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Streets_StreetId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_ApplicationUserId",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_Cities_CityId",
                table: "Suppliers");

            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_Governorates_GovernorateId",
                table: "Suppliers");

            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_Streets_StreetId",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_CityId",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_GovernorateId",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_CityId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_GovernorateId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CityId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_GovernorateId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "GovernorateId",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "GovernorateId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "GovernorateId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "StreetId",
                table: "Suppliers",
                newName: "AddressId");

            migrationBuilder.RenameIndex(
                name: "IX_Suppliers_StreetId",
                table: "Suppliers",
                newName: "IX_Suppliers_AddressId");

            migrationBuilder.RenameColumn(
                name: "StreetId",
                table: "Customers",
                newName: "AddressId");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_StreetId",
                table: "Customers",
                newName: "IX_Customers_AddressId");

            migrationBuilder.RenameColumn(
                name: "StreetId",
                table: "AspNetUsers",
                newName: "AddressId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_StreetId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_AddressId");

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GovernorateId = table.Column<int>(type: "int", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressDetails = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Addresses_Governorates_GovernorateId",
                        column: x => x.GovernorateId,
                        principalTable: "Governorates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2fe9d316-b5b7-40e9-9c54-23114c21145c",
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 8, 21, 18, 29, 16, 552, DateTimeKind.Utc).AddTicks(7075), "AQAAAAIAAYagAAAAEDA/vjCBLYcHqX8kVSLlDA5nbyL1SjQMf+IZ6uh1r+aipIsNWpMWarLVRUFs0LDGDA==" });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CityId",
                table: "Addresses",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_GovernorateId",
                table: "Addresses",
                column: "GovernorateId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Addresses_AddressId",
                table: "AspNetUsers",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Addresses_AddressId",
                table: "Customers",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_ApplicationUserId",
                table: "RefreshTokens",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Suppliers_Addresses_AddressId",
                table: "Suppliers",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Addresses_AddressId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Addresses_AddressId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_ApplicationUserId",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_Addresses_AddressId",
                table: "Suppliers");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.RenameColumn(
                name: "AddressId",
                table: "Suppliers",
                newName: "StreetId");

            migrationBuilder.RenameIndex(
                name: "IX_Suppliers_AddressId",
                table: "Suppliers",
                newName: "IX_Suppliers_StreetId");

            migrationBuilder.RenameColumn(
                name: "AddressId",
                table: "Customers",
                newName: "StreetId");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_AddressId",
                table: "Customers",
                newName: "IX_Customers_StreetId");

            migrationBuilder.RenameColumn(
                name: "AddressId",
                table: "AspNetUsers",
                newName: "StreetId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_AddressId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_StreetId");

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Suppliers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GovernorateId",
                table: "Suppliers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GovernorateId",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GovernorateId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2fe9d316-b5b7-40e9-9c54-23114c21145c",
                columns: new[] { "CityId", "CreatedAt", "GovernorateId", "PasswordHash" },
                values: new object[] { null, new DateTime(2025, 8, 21, 15, 46, 52, 864, DateTimeKind.Utc).AddTicks(1576), null, "AQAAAAIAAYagAAAAEDoF5UXS9Wy5/cxBcgp+UDsMYokoZhNJILmndgotMe2U6JQyE2Gjv4V5ejMdYedGcA==" });

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_CityId",
                table: "Suppliers",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_GovernorateId",
                table: "Suppliers",
                column: "GovernorateId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CityId",
                table: "Customers",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_GovernorateId",
                table: "Customers",
                column: "GovernorateId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CityId",
                table: "AspNetUsers",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_GovernorateId",
                table: "AspNetUsers",
                column: "GovernorateId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Cities_CityId",
                table: "AspNetUsers",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Governorates_GovernorateId",
                table: "AspNetUsers",
                column: "GovernorateId",
                principalTable: "Governorates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Streets_StreetId",
                table: "AspNetUsers",
                column: "StreetId",
                principalTable: "Streets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Cities_CityId",
                table: "Customers",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Governorates_GovernorateId",
                table: "Customers",
                column: "GovernorateId",
                principalTable: "Governorates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Streets_StreetId",
                table: "Customers",
                column: "StreetId",
                principalTable: "Streets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_ApplicationUserId",
                table: "RefreshTokens",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Suppliers_Cities_CityId",
                table: "Suppliers",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Suppliers_Governorates_GovernorateId",
                table: "Suppliers",
                column: "GovernorateId",
                principalTable: "Governorates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Suppliers_Streets_StreetId",
                table: "Suppliers",
                column: "StreetId",
                principalTable: "Streets",
                principalColumn: "Id");
        }
    }
}
