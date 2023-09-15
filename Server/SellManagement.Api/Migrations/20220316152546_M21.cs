using Microsoft.EntityFrameworkCore.Migrations;

namespace SellManagement.Api.Migrations
{
    public partial class M21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ForControlStatus",
                table: "TblSellOrderHeads",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForControlStatus",
                table: "TblSellOrderHeads");
        }
    }
}
