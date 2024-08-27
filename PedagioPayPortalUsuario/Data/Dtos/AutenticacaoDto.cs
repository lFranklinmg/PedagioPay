namespace PedagioPayFadamiBack.Data.Dto
{
    public class AutenticacaoDto
    {
        public string? Login { get; set; }
        public string Senha { get; set; }
        public string? TipoLogin { get; set; }
        public string? Token { get; set; }
    }
}
