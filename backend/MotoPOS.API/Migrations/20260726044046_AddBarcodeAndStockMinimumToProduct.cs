using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotoPOS.API.Migrations
{
    /// <inheritdoc />
    public partial class AddBarcodeAndStockMinimumToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodigoBarras",
                table: "productos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "StockMinimo",
                table: "productos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoBarras",
                table: "productos");

            migrationBuilder.DropColumn(
                name: "StockMinimo",
                table: "productos");
        }
    }
}
