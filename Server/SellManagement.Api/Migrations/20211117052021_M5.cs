using Microsoft.EntityFrameworkCore.Migrations;

namespace SellManagement.Api.Migrations
{
    public partial class M5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductBarcode",
                table: "TblProducts");

            migrationBuilder.DropColumn(
                name: "ProductCategory",
                table: "TblProducts");

            migrationBuilder.RenameColumn(
                name: "ProductTrademark",
                table: "TblProducts",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "ProductSoldPrice",
                table: "TblProducts",
                newName: "TradeMarkCd");

            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "TblProducts",
                newName: "Detail");

            migrationBuilder.RenameColumn(
                name: "ProductDetail",
                table: "TblProducts",
                newName: "Barcode");

            migrationBuilder.RenameColumn(
                name: "ProductCostPrice",
                table: "TblProducts",
                newName: "SoldPrice");

            migrationBuilder.AddColumn<int>(
                name: "CategoryCd",
                table: "TblProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CostPrice",
                table: "TblProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OriginCd",
                table: "TblProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TblClassifiesName",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblClassifiesName", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblClassifiesName");

            migrationBuilder.DropColumn(
                name: "CategoryCd",
                table: "TblProducts");

            migrationBuilder.DropColumn(
                name: "CostPrice",
                table: "TblProducts");

            migrationBuilder.DropColumn(
                name: "OriginCd",
                table: "TblProducts");

            migrationBuilder.RenameColumn(
                name: "TradeMarkCd",
                table: "TblProducts",
                newName: "ProductSoldPrice");

            migrationBuilder.RenameColumn(
                name: "SoldPrice",
                table: "TblProducts",
                newName: "ProductCostPrice");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "TblProducts",
                newName: "ProductTrademark");

            migrationBuilder.RenameColumn(
                name: "Detail",
                table: "TblProducts",
                newName: "ProductName");

            migrationBuilder.RenameColumn(
                name: "Barcode",
                table: "TblProducts",
                newName: "ProductDetail");

            migrationBuilder.AddColumn<string>(
                name: "ProductBarcode",
                table: "TblProducts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductCategory",
                table: "TblProducts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
