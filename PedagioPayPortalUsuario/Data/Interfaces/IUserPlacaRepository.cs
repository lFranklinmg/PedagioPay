using PedagioPayFadamiBack.Data.Dto;
using PedagioPayPortalUsuario.Models;

namespace PedagioPayFadamiBack.Data.Repository.Interface
{
    public interface IUserPlacaRepository
    {
        public PLACAs Cadastra(PLACAs novaPlaca);
        //public List<UserPlacaDto> BuscarPlaca(string userToken);
        public Task ExcluirPlaca(int idUserPlaca);
    }
}
