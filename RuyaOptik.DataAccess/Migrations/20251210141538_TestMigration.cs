using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RuyaOptik.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class TestMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "31e5a815-695b-48be-aab7-a30dd4b8ea09");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ded4f56b-1d80-48fa-a07c-24d952a2b6c7");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "d04e1c58-5a27-4d07-ae58-a2f4bb9e4fb6", null, "User", "USER" },
                    { "daebea62-1371-4a6e-9684-ec3450d58027", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d04e1c58-5a27-4d07-ae58-a2f4bb9e4fb6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "daebea62-1371-4a6e-9684-ec3450d58027");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "31e5a815-695b-48be-aab7-a30dd4b8ea09", null, "Admin", "ADMIN" },
                    { "ded4f56b-1d80-48fa-a07c-24d952a2b6c7", null, "User", "USER" }
                });
        }
    }
}
