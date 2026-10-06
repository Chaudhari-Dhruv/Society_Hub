using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Society_hub.Migrations
{
    /// <inheritdoc />
    public partial class MakeResidentFlatOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Residents_Flats_FlatId",
                table: "Residents");

            migrationBuilder.AlterColumn<int>(
                name: "FlatId",
                table: "Residents",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Residents_Flats_FlatId",
                table: "Residents",
                column: "FlatId",
                principalTable: "Flats",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Residents_Flats_FlatId",
                table: "Residents");

            migrationBuilder.AlterColumn<int>(
                name: "FlatId",
                table: "Residents",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Residents_Flats_FlatId",
                table: "Residents",
                column: "FlatId",
                principalTable: "Flats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
