using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Comp1640.Migrations
{
    /// <inheritdoc />
    public partial class updateadadad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "dbo",
                table: "AcademicYear",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                schema: "dbo",
                table: "AcademicYear");
        }
    }
}
