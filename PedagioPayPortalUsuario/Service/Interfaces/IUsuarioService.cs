using PedagioPayFadamiBack.Data.Dto;
using PedagioPayPortalUsuario.Data.Dtos;
using PedagioPayPortalUsuario.Data.Dtos.Usuario;
using PedagioPayPortalUsuario.Models;

namespace PedagioPayPortalUsuario.Service.Interfaces
{
    public interface IUsuarioService
    {
        public Task Cadastra(USUARIO usuario);
        public Task CadastraExterno(USUARIO usuarioExterno);
        public Task Atualiza(UpdateUsuarioDto usuario);
        public bool BuscaUsuario(string login, string campo, string token);
        public bool AtualizaSenha(string novaSenha, int tokenUsuario, string emailUsuario);
        public bool VerificaEmail(string email);
        public bool VerificaSenha(string login, string campo, string senha);
        public bool VerificaAtivo(string login, string campo, string token);
        public string EncryptSenha(string senha);
        public bool VerificaCadastro(string login);
        string TokenUsuario(string email, string campo);
        string GetTokenValidate(string email);
        UserDto GetUsuario(string token);
        public int BuscarIdUsuarioPorEmail(string emailUsuario);
    }
}
