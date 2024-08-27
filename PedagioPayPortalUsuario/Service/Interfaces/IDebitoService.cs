using PedagioPayPortalUsuario.Data.Dtos.Debitos;
using PedagioPayPortalUsuario.Models;

namespace PedagioPayPortalUsuario.Service.Interfaces
{
    public interface IDebitoService
    {
        public Task AdicionaDebitos(AddDebitosDto debitos);
        public List<DebitosDto> BuscaDebitos(string tokenConcessao, string token);
        public List<DebitosDto> HistoricoDebitos(string tokenConcessao, string token);
        public List<DebitosDto> ConsultaAvulsa(string tokenConcessao, ConsultaAvulsaDto consulta);
    }
}
