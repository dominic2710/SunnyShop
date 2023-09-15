using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SellManagement.Api.Migrations
{
    public partial class M12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PurchaseOrderCd",
                table: "TblPurchaseOrderHeads",
                newName: "PurchaseOrderNo");

            migrationBuilder.RenameColumn(
                name: "PurchaseOrderCd",
                table: "TblPurchaseOrderDetails",
                newName: "PurchaseOrderNo");

            migrationBuilder.CreateTable(
                name: "TblVoucherNoManagements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryCd = table.Column<int>(type: "int", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VoucherNo = table.Column<int>(type: "int", nullable: false),
                    CreateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblVoucherNoManagements", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblVoucherNoManagements");

            migrationBuilder.RenameColumn(
                name: "PurchaseOrderNo",
                table: "TblPurchaseOrderHeads",
                newName: "PurchaseOrderCd");

            migrationBuilder.RenameColumn(
                name: "PurchaseOrderNo",
                table: "TblPurchaseOrderDetails",
                newName: "PurchaseOrderCd");
        }
    }
}
