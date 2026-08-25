using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotoPOS.API.Migrations
{
    /// <inheritdoc />
    public partial class AddContrasenaHashToUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Contrasena",
                table: "usuarios",
                newName: "ContrasenaHash");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContrasenaHash",
                table: "usuarios",
                newName: "Contrasena");
        }
    }
}
