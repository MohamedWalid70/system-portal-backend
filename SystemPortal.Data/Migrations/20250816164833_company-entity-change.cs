using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SystemPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class companyentitychange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OtpValue",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtpValue",
                table: "AspNetUsers");
        }
    }
}
