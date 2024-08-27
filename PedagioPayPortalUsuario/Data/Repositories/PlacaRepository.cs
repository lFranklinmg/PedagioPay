using PedagioPayPortalUsuario.Data.Dtos;
using PedagioPayPortalUsuario.Data.Interfaces;
using PedagioPayPortalUsuario.Models;
using System.Numerics;

namespace PedagioPayPortalUsuario.Data.Repositories
{
    public class PlacaRepository : IRepositoryPlaca
    {
        private readonly PEDAGIOPAYContext _context;
        public PlacaRepository(PEDAGIOPAYContext context)
        {
            _context = context;
        }
        public PLACAs Cadastra(PLACAs usuarioPlaca)
        {
            try
            {
                var consultaUsuario = _context.USUARIOs
                    .Where(x => x.TOKEN == usuarioPlaca.TOKEN_USUARIO)
                    .Select(e => e.EMAIL).FirstOrDefault();

                var consultaPlaca = _context.PLACAs.Where(u => u.USUARIO_EMAIL == consultaUsuario)
                    .Select(p => p.PLACA).ToList();

                if (!consultaPlaca.Contains(usuarioPlaca.PLACA))
                {

                    usuarioPlaca.USUARIO_EMAIL = consultaUsuario;
                    _context.Add(usuarioPlaca);
                    _context.SaveChanges();
                    return usuarioPlaca;
                }
                else
                {
                    throw new Exception("Placa já está vinculada ao usuário.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao cadastrar placa.", ex);
            }
        }

        public bool AtualizaPlaca(UpdatePlacaDto updatePlaca)
        {

            var usuario = _context.PLACAs.FirstOrDefault(x => x.TOKEN_USUARIO == updatePlaca.Token);
            if (usuario != null)
            {
                usuario.PLACA = updatePlaca.Placa;
                usuario.USUARIO_EMAIL = updatePlaca.Email;
                _context.SaveChanges();
                return true;
            }
            else
            {
                throw new Exception("Usuário não encontrado");
            }

        }

        public decimal BuscarDebitos(string placa)
        {
            try
            {
                if (placa != null)
                {
                    var consulta = _context.PASSAGEMs
                        .Where(x => x.PLACA == placa)
                        .Select(p => p.VALOR)
                        .FirstOrDefault();

                    return consulta;
                }
                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public List<PlacaDto> BuscarPlaca(string usuarioPlaca)
        {
            try
            {
                var consultaUsuario = _context.USUARIOs
                    .Where(x => x.TOKEN == usuarioPlaca)
                    .Select(e => e.TOKEN)
                    .FirstOrDefault();

                var consultaPlaca = _context.PLACAs
                    .Where(x => x.TOKEN_USUARIO == consultaUsuario)
                    .Select(p => new PlacaDto { Id = p.ID_USUARIO_PLACA, Placa = p.PLACA, Ano = p.ANO, Cor = p.COR, Marca = p.MARCA, Modelo = p.MODELO })
                    .ToList();
                if (consultaPlaca != null)
                {
                    return consultaPlaca;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao encontrar placa", ex);
            }
        }

        public Task DeletarPlaca(int idPlaca)
        {
            try
            {
                var usuarioPlaca = _context.PLACAs.FirstOrDefault(p => p.ID_USUARIO_PLACA == idPlaca);
                _context.Remove(usuarioPlaca);
                _context.SaveChanges();
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir placa");

            }
        }
    }
}
