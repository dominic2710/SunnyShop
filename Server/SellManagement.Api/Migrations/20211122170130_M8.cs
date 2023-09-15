using Microsoft.EntityFrameworkCore.Migrations;

namespace SellManagement.Api.Migrations
{
    public partial class M8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TblProductCombos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductComboCd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductCd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quatity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblProductCombos", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblProductCombos");
        }
    }
}
