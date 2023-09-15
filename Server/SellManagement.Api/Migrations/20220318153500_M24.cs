using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SellManagement.Api.Migrations
{
    public partial class M24 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CancelDate",
                table: "TblSellOrderHeads",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancelDate",
                table: "TblSellOrderHeads");
        }
    }
}
