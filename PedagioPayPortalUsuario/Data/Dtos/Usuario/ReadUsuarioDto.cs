using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PedagioPayPortalUsuario.Data.Dtos.Usuario
{
    public class ReadUsuarioDto
    {
        public int ID_USUARIO { get; set; }
        public string NOME { get; set; }
        public string EMAIL { get; set; }
        public string CPF_CNPJ { get; set; }
        public string CELULAR { get; set; }
        public string SENHA { get; set; }
        public bool? BL_VALIDADO { get; set; }
        public bool? CD_STATUS { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DH_TIMESTAMP { get; set; }
        public string ID_FACEBOOK { get; set; }
        public string ID_GOOGLE { get; set; }
        public string ID_APPLE { get; set; }
    }
}
