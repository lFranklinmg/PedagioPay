using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PedagioPayPortalUsuario.Data.Dtos.Usuario
{
    public class CreateUsuarioDto
    {
        public int ID_USUARIO { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string NOME { get; set; }
        [Key]
        [StringLength(50)]
        [Unicode(false)]
        public string EMAIL { get; set; }
        [Required]
        [StringLength(20)]
        [Unicode(false)]
        public string CPF_CNPJ { get; set; }
        [StringLength(12)]
        [Unicode(false)]
        public string? CELULAR { get; set; }
        [StringLength(32)]
        [Unicode(false)]
        public string SENHA { get; set; }
        public bool? BL_VALIDADO { get; set; }
        public bool? CD_STATUS { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DH_TIMESTAMP { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string? ID_FACEBOOK { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string? ID_GOOGLE { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string? ID_APPLE { get; set; }
    }
}
