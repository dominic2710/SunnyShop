using Microsoft.EntityFrameworkCore.Migrations;

namespace SellManagement.Api.Migrations
{
    public partial class M15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SaleCost",
                table: "TblPurchaseOrderHeads",
                newName: "SaleOffCost");

            migrationBuilder.AddColumn<int>(
                name: "PaidCost",
                table: "TblPurchaseOrderHeads",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaidCost",
                table: "TblPurchaseOrderHeads");

            migrationBuilder.RenameColumn(
                name: "SaleOffCost",
                table: "TblPurchaseOrderHeads",
                newName: "SaleCost");
        }
    }
}
