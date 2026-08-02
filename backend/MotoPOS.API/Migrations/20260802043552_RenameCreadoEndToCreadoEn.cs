using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotoPOS.API.Migrations
{
    /// <inheritdoc />
    public partial class RenameCreadoEndToCreadoEn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreadoEnd",
                table: "ventas",
                newName: "CreadoEn");

            migrationBuilder.RenameColumn(
                name: "CreadoEnd",
                table: "usuarios",
                newName: "CreadoEn");

            migrationBuilder.RenameColumn(
                name: "CreadoEnd",
                table: "proveedores",
                newName: "CreadoEn");

            migrationBuilder.RenameColumn(
                name: "CreadoEnd",
                table: "productos",
                newName: "CreadoEn");

            migrationBuilder.RenameColumn(
                name: "CreadoEnd",
                table: "marcas",
                newName: "CreadoEn");

            migrationBuilder.RenameColumn(
                name: "CreadoEnd",
                table: "compras",
                newName: "CreadoEn");

            migrationBuilder.RenameColumn(
                name: "CreadoEnd",
                table: "clientes",
                newName: "CreadoEn");

            migrationBuilder.RenameColumn(
                name: "CreadoEnd",
                table: "categorias",
                newName: "CreadoEn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreadoEn",
                table: "ventas",
                newName: "CreadoEnd");

            migrationBuilder.RenameColumn(
                name: "CreadoEn",
                table: "usuarios",
                newName: "CreadoEnd");

            migrationBuilder.RenameColumn(
                name: "CreadoEn",
                table: "proveedores",
                newName: "CreadoEnd");

            migrationBuilder.RenameColumn(
                name: "CreadoEn",
                table: "productos",
                newName: "CreadoEnd");

            migrationBuilder.RenameColumn(
                name: "CreadoEn",
                table: "marcas",
                newName: "CreadoEnd");

            migrationBuilder.RenameColumn(
                name: "CreadoEn",
                table: "compras",
                newName: "CreadoEnd");

            migrationBuilder.RenameColumn(
                name: "CreadoEn",
                table: "clientes",
                newName: "CreadoEnd");

            migrationBuilder.RenameColumn(
                name: "CreadoEn",
                table: "categorias",
                newName: "CreadoEnd");
        }
    }
}
