using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Certitrack.Migrations
{
    /// <inheritdoc />
    public partial class AddSchools : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ApprovalToken",
                value: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ApprovalToken",
                value: null);
        }
    }
}
