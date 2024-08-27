using System;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PedagioPayPortalUsuario.Migrations
{
    /// <inheritdoc />
    public partial class TabelaUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        { 
            /*migrationBuilder.CreateSequence<short>(
                name: "SQ_PEDIDO_MERCHANT_ORDER_ID",
                minValue: 1L,
                maxValue: 9999L,
                cyclic: true);
            
            migrationBuilder.CreateTable(
                name: "USUARIO",
                columns: table => new 
                {
                    EMAIL = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    IDUSUARIO = table.Column<int>(name: "ID_USUARIO", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NOME = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    CPFCNPJ = table.Column<string>(name: "CPF_CNPJ", type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    CELULAR = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: true),
                    SENHA = table.Column<string>(type: "varchar(32)", unicode: false, maxLength: 32, nullable: true),
                    BLVALIDADO = table.Column<bool>(name: "BL_VALIDADO", type: "bit", nullable: true, defaultValueSql: "((0))"),
                    CODIGOVALIDACAO = table.Column<bool>(name: "CODIGO_VALIDACAO", type: "char(4)", nullable: true, unicode: false),
                    CDSTATUS = table.Column<bool>(name: "CD_STATUS", type: "bit", nullable: true, defaultValueSql: "((1))"),
                    DHTIMESTAMP = table.Column<DateTime>(name: "DH_TIMESTAMP", type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    IDFACEBOOK = table.Column<string>(name: "ID_FACEBOOK", type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    IDGOOGLE = table.Column<string>(name: "ID_GOOGLE", type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    IDAPPLE = table.Column<string>(name: "ID_APPLE", type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIO", x => x.EMAIL);
                });*/
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.DropTable(
                name: "USUARIO");

            migrationBuilder.DropSequence(
                name: "SQ_PEDIDO_MERCHANT_ORDER_ID");*/
        }
    }
}
