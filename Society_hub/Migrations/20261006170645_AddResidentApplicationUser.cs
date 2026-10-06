using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Society_hub.Migrations
{
    /// <inheritdoc />
    public partial class AddResidentApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Residents",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Residents_ApplicationUserId",
                table: "Residents",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Residents_AspNetUsers_ApplicationUserId",
                table: "Residents",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Residents_AspNetUsers_ApplicationUserId",
                table: "Residents");

            migrationBuilder.DropIndex(
                name: "IX_Residents_ApplicationUserId",
                table: "Residents");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Residents");
        }
    }
}
