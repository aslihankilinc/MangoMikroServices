using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Mango.Services.AuthApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "59fdb24a-2ce9-4612-927a-f68c22e10778");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cd88974c-8929-4036-b5dd-6a54b6d064ed");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "477e7c33-d4b7-4466-9c35-1c9c8a0b656a", null, "CUSTOMER", "CUSTOMER" },
                    { "ff1db8e8-7c65-471f-861f-7bf5c1f75fe2", null, "ADMIN", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "477e7c33-d4b7-4466-9c35-1c9c8a0b656a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ff1db8e8-7c65-471f-861f-7bf5c1f75fe2");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "59fdb24a-2ce9-4612-927a-f68c22e10778", null, "ADMIN", "ADMIN" },
                    { "cd88974c-8929-4036-b5dd-6a54b6d064ed", null, "CUSTOMER", "CUSTOMER" }
                });
        }
    }
}
