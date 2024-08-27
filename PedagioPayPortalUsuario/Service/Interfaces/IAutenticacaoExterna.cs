using PedagioPayPortalUsuario.Data.Dtos.AutenticacaoExterna;

namespace PedagioPayPortalUsuario.Service.Interfaces
{
    public interface IAutenticacaoExterna
    {
        public Task<string> GetUsuario(string url);

    }
}
