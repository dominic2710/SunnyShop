using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SellManagement.Api.Migrations
{
    public partial class M11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TblPurchaseOrderDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseOrderCd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductCd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Qualtity = table.Column<int>(type: "int", nullable: false),
                    CostPrice = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblPurchaseOrderDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TblPurchaseOrderHeads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseOrderCd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseOrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlannedImportDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SupplierCd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SummaryCost = table.Column<int>(type: "int", nullable: false),
                    SaleCost = table.Column<int>(type: "int", nullable: false),
                    PurchaseCost = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblPurchaseOrderHeads", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblPurchaseOrderDetails");

            migrationBuilder.DropTable(
                name: "TblPurchaseOrderHeads");
        }
    }
}
