using PedagioPayPortalUsuario.Data.Dtos.Debitos;
using PedagioPayPortalUsuario.Models;

namespace PedagioPayPortalUsuario.Data.Interfaces
{
    public interface IRepositoryDebito
    {
        public Task Cadastra (DEBITO usuarioDebitos);
        public List<DebitosDto> Buscar(string tokenConcessao, string token);
        public List<DebitosDto> HistoricoDebitos(string tokenConcessao, string token);
        public List<DebitosDto> ConsultaAvulsa(string tokenConcessao, ConsultaAvulsaDto consulta);
    }
}
