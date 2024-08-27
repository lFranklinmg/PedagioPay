using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PedagioPayPortalUsuario.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDebitoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PAGO_EM",
                table: "DEBITO",
                type: "datetime",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PAGO_EM",
                table: "DEBITO");
        }
    }
}
