using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace powerbiembeddedapi.Migrations
{
    /// <inheritdoc />
    public partial class AddedStatusColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "AccessRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "AccessRequests");
        }
    }
}
