using System.Globalization;

namespace PedagioPayFadamiBack.Data.Dto
{
    public class UserDto
    {
        public  int? Id { get; set; }
        public string? Nome { get; set; }

        public string? Email { get; set; }

        public string? Cpf_Cnpj { get; set; }

        public string? Celular { get; set; }

        public string? Senha { get; set; }

        public bool? Bl_Validado { get; set; }

        public string? Codigo_Validacao { get; set; }

        public string? Token { get; set; }

        public string? Id_Google { get; set; }
        public string? TipoLogin { get; set; }
    }
}
