using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Comp1640.Migrations
{
    /// <inheritdoc />
    public partial class adada : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contributions_Faculties_FacId",
                schema: "dbo",
                table: "Contributions");

            migrationBuilder.DropIndex(
                name: "IX_Contributions_FacId",
                schema: "dbo",
                table: "Contributions");

            migrationBuilder.DropColumn(
                name: "FacId",
                schema: "dbo",
                table: "Contributions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FacId",
                schema: "dbo",
                table: "Contributions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contributions_FacId",
                schema: "dbo",
                table: "Contributions",
                column: "FacId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contributions_Faculties_FacId",
                schema: "dbo",
                table: "Contributions",
                column: "FacId",
                principalSchema: "dbo",
                principalTable: "Faculties",
                principalColumn: "FacID");
        }
    }
}
