using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SystemPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class otptablechange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOtpVerified",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "OtpId",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Otps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<int>(type: "integer", nullable: false),
                    TransmissionTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false),
                    VerificationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TimeToExpireInSeconds = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Otps", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_OtpId",
                table: "AspNetUsers",
                column: "OtpId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Otps_OtpId",
                table: "AspNetUsers",
                column: "OtpId",
                principalTable: "Otps",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Otps_OtpId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Otps");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_OtpId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsOtpVerified",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OtpId",
                table: "AspNetUsers");
        }
    }
}
