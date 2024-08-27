namespace PedagioPayFadamiBack.Data.Dto
{
    public class CadastroDto
    {
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Cpf_Cnpj { get; set; }
        public string? Celular { get; set; }
        public string? Senha { get; set; }
        public string? Placa { get; set; }
        public string? Marca_Modelo { get; set; }
        public string? Token {  get; set; }
        public string TokenConcessao { get; set; }
    }
}
