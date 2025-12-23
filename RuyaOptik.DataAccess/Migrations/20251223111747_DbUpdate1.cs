using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RuyaOptik.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class DbUpdate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "3e3d6759-3b3d-4362-bcbb-71118a067cbb");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "6f4aa0cd-d23a-4db2-b187-6b0df6de2d54");

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b0dafad2-0482-4dd1-a8f9-d5b7191ba6b5", null, "User", "USER" },
                    { "e700bd96-a81c-4df7-b1d9-b65bc344e593", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "b0dafad2-0482-4dd1-a8f9-d5b7191ba6b5");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "e700bd96-a81c-4df7-b1d9-b65bc344e593");

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3e3d6759-3b3d-4362-bcbb-71118a067cbb", null, "Admin", "ADMIN" },
                    { "6f4aa0cd-d23a-4db2-b187-6b0df6de2d54", null, "User", "USER" }
                });
        }
    }
}
