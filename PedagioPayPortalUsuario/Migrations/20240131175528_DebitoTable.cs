using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PedagioPayPortalUsuario.Migrations
{
    /// <inheritdoc />
    public partial class DebitoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           /* migrationBuilder.CreateTable(
                name: "DEBITO",
                columns: table => new
                {
                    IDDEBITO = table.Column<int>(name: "ID_DEBITO", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDPASSAGEM = table.Column<long>(name: "ID_PASSAGEM", type: "bigint", nullable: false),
                    CATEGORIA = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    VALOR = table.Column<decimal>(type: "smallmoney", unicode: false, nullable: false),
                    DHPASSAGEM = table.Column<DateTime>(name: "DH_PASSAGEM", type: "datetime2", nullable: false),
                    CONCESSAO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PLACA = table.Column<string>(type: "varchar(7)", unicode: false, maxLength: 7, nullable: false),
                    TAG = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    IDOSA = table.Column<byte>(name: "ID_OSA", type: "tinyint", nullable: true),
                    CDSTATUS = table.Column<bool>(name: "CD_STATUS", type: "bit", nullable: true),
                    DHTIMESTAMP = table.Column<DateTime>(name: "DH_TIMESTAMP", type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DEBITO", x => x.IDDEBITO);
                });*/        
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.DropTable(
                name: "DEBITO");*/
        }
    }
}
