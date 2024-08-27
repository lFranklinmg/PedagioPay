using PedagioPayPortalUsuario.Data.Dtos;
using PedagioPayPortalUsuario.Models;

namespace PedagioPayPortalUsuario.Data.Interfaces
{
    public interface IRepositoryPlaca
    {
        public PLACAs Cadastra(PLACAs usuarioPlaca);
        public decimal BuscarDebitos(string placa);
        public List<PlacaDto> BuscarPlaca(string usuarioPlaca);
        public bool AtualizaPlaca(UpdatePlacaDto updatePlaca);
        public Task DeletarPlaca(int idPlaca);
    }
}
