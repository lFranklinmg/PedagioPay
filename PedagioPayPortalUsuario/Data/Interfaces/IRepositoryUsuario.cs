using PedagioPayFadamiBack.Data.Dto;
using PedagioPayPortalUsuario.Data.Dtos;
using PedagioPayPortalUsuario.Data.Dtos.Usuario;
using PedagioPayPortalUsuario.Models;

namespace PedagioPayPortalUsuario.Data.Interfaces
{

    public interface IRepositoryUsuario
    {
        Task Cadastra(USUARIO usuario);
        Task CadastraExterno(USUARIO usuarioExterno);
        Task Atualiza(UpdateUsuarioDto usuario);
        public bool AtualizaSenha(string novaSenha, int tokenUsuario, string emailUsuario);
        Task<USUARIO> Busca(string login);
        bool Autentica(string login, string campo, string token);
        string BuscaSenha(string login, string campo);
        bool BuscaCodConfirmacao(string codigo);
        bool VerificaEmail(string email);
        bool VerificaAtivo(string login, string campo, string token);
        string TokenUsuario(string email, string campo);
        string GetTokenValidate(string email);
        public string GetTokenUsuario(string email);
        UserDto GetUsuario(string token);
        int BuscarIdUsuarioPorEmail(string emailUsuario);
    }
}

