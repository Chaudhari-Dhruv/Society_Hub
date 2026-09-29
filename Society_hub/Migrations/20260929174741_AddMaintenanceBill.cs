using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Society_hub.Migrations
{
    /// <inheritdoc />
    public partial class AddMaintenanceBill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillNumber",
                table: "MaintenanceBills");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "MaintenanceBills",
                newName: "PaymentStatus");

            migrationBuilder.RenameColumn(
                name: "BillDate",
                table: "MaintenanceBills",
                newName: "CreatedDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "BillingMonth",
                table: "MaintenanceBills",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "MaintenanceBills",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceiptNumber",
                table: "MaintenanceBills",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillingMonth",
                table: "MaintenanceBills");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "MaintenanceBills");

            migrationBuilder.DropColumn(
                name: "ReceiptNumber",
                table: "MaintenanceBills");

            migrationBuilder.RenameColumn(
                name: "PaymentStatus",
                table: "MaintenanceBills",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "MaintenanceBills",
                newName: "BillDate");

            migrationBuilder.AddColumn<string>(
                name: "BillNumber",
                table: "MaintenanceBills",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
