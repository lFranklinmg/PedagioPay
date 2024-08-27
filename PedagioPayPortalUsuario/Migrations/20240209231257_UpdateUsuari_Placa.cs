using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PedagioPayPortalUsuario.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUsuariPlaca : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TOKEN_USUARIO",
                table: "USUARIO_PLACA",
                type: "varchar(500)",
                unicode: false,
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TOKEN_USUARIO",
                table: "USUARIO_PLACA");
        }
    }
}
