using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SystemPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class companyedit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LogoContentType",
                table: "Companies",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoFileName",
                table: "Companies",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoContentType",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "LogoFileName",
                table: "Companies");
        }
    }
}
