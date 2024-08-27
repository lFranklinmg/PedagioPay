using PedagioPayFadamiBack.Data.Dto;
using PedagioPayFadamiBack.Data.Repository.Interface;
using PedagioPayPortalUsuario.Data.Dtos;
using PedagioPayPortalUsuario.Models;

namespace PedagioPayFadamiBack.Data.Repository
{
    public class UserPlacaRepository : IUserPlacaRepository
    {
        private readonly PEDAGIOPAYContext _context;
        private readonly IConfiguration _configuration;
        public UserPlacaRepository(IConfiguration configuration, PEDAGIOPAYContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        public PLACAs Cadastra(PLACAs novaPlaca)
        {
            try
            {
                _context.Add(novaPlaca);
                _context.SaveChanges();
                return novaPlaca;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro adicionar veículo na base.", ex);
            }
        }
        public List<PlacaDto> BuscarPlaca(string userToken)
        {
            try
            {
                var consultaUsuario = _context.USUARIOs
                    .Where(x => x.TOKEN == userToken)
                    .Select(e => e.TOKEN)
                    .FirstOrDefault();

                var consultaPlaca = _context.PLACAs
                    .Where(x => x.TOKEN_USUARIO == consultaUsuario)
                    .Select(p => new PlacaDto { Id = p.ID_USUARIO_PLACA, Placa = p.PLACA, Ano = p.ANO, Cor = p.COR, Marca = p.MARCA, Modelo = p.MODELO })
                    .ToList();
                return consultaPlaca;
            }
            catch (Exception ex) 
            {
                throw new Exception();
            }
        }
        public Task ExcluirPlaca(int idUserPlaca)
        {
            try
            {
                var usuarioPlaca = _context.PLACAs.FirstOrDefault(p => p.ID_USUARIO_PLACA == idUserPlaca);
                _context.Remove(usuarioPlaca);
                _context.SaveChanges();
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

    }
}
