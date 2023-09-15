using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SellManagement.Api.Migrations
{
    public partial class M22 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CollectOnDeliveryCash",
                table: "TblSellOrderHeads",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryDate",
                table: "TblSellOrderHeads",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "PlannedDeliveryDate",
                table: "TblSellOrderHeads",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "PlannedPickingDate",
                table: "TblSellOrderHeads",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "WaybillCost",
                table: "TblSellOrderHeads",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WaybillStatus",
                table: "TblSellOrderHeads",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "WaybillVoucherNo",
                table: "TblSellOrderHeads",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CollectOnDeliveryCash",
                table: "TblSellOrderHeads");

            migrationBuilder.DropColumn(
                name: "DeliveryDate",
                table: "TblSellOrderHeads");

            migrationBuilder.DropColumn(
                name: "PlannedDeliveryDate",
                table: "TblSellOrderHeads");

            migrationBuilder.DropColumn(
                name: "PlannedPickingDate",
                table: "TblSellOrderHeads");

            migrationBuilder.DropColumn(
                name: "WaybillCost",
                table: "TblSellOrderHeads");

            migrationBuilder.DropColumn(
                name: "WaybillStatus",
                table: "TblSellOrderHeads");

            migrationBuilder.DropColumn(
                name: "WaybillVoucherNo",
                table: "TblSellOrderHeads");
        }
    }
}
