using PedagioPayFadamiBack.Data.Dto;
using PedagioPayPortalUsuario.Models;

namespace PedagioPayFadamiBack.Data.Repository.Interface
{
    public interface IUserRepository
    {
        public Task<UserDto> Cadastra(USUARIO novoUsuario);
        public Task<UserDto> Autentica(AutenticacaoDto login);
        public bool confirmacaoCadastro(string tokenValidacao);
        public string ReenviaTokenValidacao(string userEmail, string tokenUsuario);
    }
}
