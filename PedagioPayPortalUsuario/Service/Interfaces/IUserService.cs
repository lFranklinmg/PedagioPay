using PedagioPayFadamiBack.Data.Dto;

namespace PedagioPayFadamiBack.Service.Interface
{
    public interface IUserService
    {
        public Task<UserDto> Cadastra(string tokenConcessao, CadastroDto novoUsuario);
        public Task<UserDto> Autentica(AutenticacaoDto login);
        public bool ConfirmacaoCadastro(string tokenValidacao);
        public string ReenviaTokenValidacao(string tokenConcessao, string userEmail, string tokenUsuario);
        public string EncryptSenha(string senha);
    }
}
