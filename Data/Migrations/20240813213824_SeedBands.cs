using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Test.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedBands : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Bands",
                columns: new[] { "Id", "Country", "FormedIn", "Name" },
                values: new object[,]
                {
                    { 1, null, null, "Bethlehem" },
                    { 2, null, null, "Devourment" },
                    { 3, null, null, "Pink Floyd" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Bands",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Bands",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Bands",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
