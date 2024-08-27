using PedagioPayPortalUsuario.Data.Dtos;
using PedagioPayPortalUsuario.Models;

namespace PedagioPayPortalUsuario.Service.Interfaces
{
    public interface IPlacaService
    {
        public PLACAs Cadastra(PLACAs usuarioPlaca);
        public List<PlacaDto> BuscarPlaca(string usuarioPlaca);
        public Task ExcluirPlaca(int idPlaca);
        public bool AtualizaPlaca(UpdatePlacaDto updatePlaca);
    }
}
