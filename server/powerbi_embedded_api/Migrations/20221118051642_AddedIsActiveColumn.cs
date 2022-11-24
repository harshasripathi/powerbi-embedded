using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace powerbiembeddedapi.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsActiveColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AccessRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AccessRequests");
        }
    }
}
