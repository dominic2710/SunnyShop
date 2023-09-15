using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SellManagement.Api.Migrations
{
    public partial class M20 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TblSellOrderDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SellOrderNo = table.Column<string>(type: "text", nullable: true),
                    ProductCd = table.Column<string>(type: "text", nullable: true),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    CostPrice = table.Column<int>(type: "integer", nullable: false),
                    Cost = table.Column<int>(type: "integer", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblSellOrderDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TblSellOrderHeads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SellOrderNo = table.Column<string>(type: "text", nullable: true),
                    SellOrderDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    PlannedExportDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CustomerCd = table.Column<string>(type: "text", nullable: true),
                    ShippingCompanyCd = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    SummaryCost = table.Column<int>(type: "integer", nullable: false),
                    ShippingCost = table.Column<int>(type: "integer", nullable: false),
                    SaleOffCost = table.Column<int>(type: "integer", nullable: false),
                    PaidCost = table.Column<int>(type: "integer", nullable: false),
                    SellCost = table.Column<int>(type: "integer", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    CreateUserId = table.Column<string>(type: "text", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdateUserId = table.Column<string>(type: "text", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblSellOrderHeads", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TblShippingCompanys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ShippingCompanyCd = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Address1 = table.Column<string>(type: "text", nullable: true),
                    Address2 = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblShippingCompanys", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblSellOrderDetails");

            migrationBuilder.DropTable(
                name: "TblSellOrderHeads");

            migrationBuilder.DropTable(
                name: "TblShippingCompanys");
        }
    }
}
