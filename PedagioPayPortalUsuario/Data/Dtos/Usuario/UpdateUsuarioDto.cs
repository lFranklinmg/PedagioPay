using System.ComponentModel.DataAnnotations.Schema;

namespace PedagioPayPortalUsuario.Data.Dtos.Usuario
{
    public class UpdateUsuarioDto
    {
        public string? NOME { get; set; }
        public string? EMAIL { get; set; }
        public string? CPF_CNPJ { get; set; }
        public string? CELULAR { get; set; }

    }
}
