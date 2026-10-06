using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Society_hub.Migrations
{
    /// <inheritdoc />
    public partial class AddSecurityGuardApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "SecurityGuards",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SecurityGuards_ApplicationUserId",
                table: "SecurityGuards",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SecurityGuards_AspNetUsers_ApplicationUserId",
                table: "SecurityGuards",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SecurityGuards_AspNetUsers_ApplicationUserId",
                table: "SecurityGuards");

            migrationBuilder.DropIndex(
                name: "IX_SecurityGuards_ApplicationUserId",
                table: "SecurityGuards");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "SecurityGuards");
        }
    }
}
