using PedagioPayPortalUsuario.Data.Dtos;
using PedagioPayPortalUsuario.Data.Interfaces;
using PedagioPayPortalUsuario.Models;
using PedagioPayPortalUsuario.Service.Interfaces;
using Serilog;

namespace PedagioPayPortalUsuario.Service
{
    public class PlacaService : IPlacaService
    {
        private readonly IRepositoryPlaca _repositoryPlaca;
        public PlacaService( IRepositoryPlaca repositoryPlaca)
        {
            _repositoryPlaca = repositoryPlaca;
        }

        public PLACAs Cadastra(PLACAs usuarioPlaca)
        {
            try
            {
                var novaPlaca = _repositoryPlaca.Cadastra(usuarioPlaca);
                if (novaPlaca != null)
                {
                    return novaPlaca;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Information("Erro ao cadastrar placa", ex);
                throw new Exception("Não foi possível cadastrar placa", ex);
            }
        }

        public List<PlacaDto> BuscarPlaca(string usuarioPlaca)
        {
            try
            {
               var placas = _repositoryPlaca.BuscarPlaca(usuarioPlaca);
                if(placas != null)
                {
                    return placas;
                }
                return null;
            }
            catch (Exception ex)
            {
                Log.Information("Erro ao buscar placa", ex);
                throw new Exception("Erro ao buscar placa.", ex);
            }
        }

        public Task ExcluirPlaca(int idPlaca)
        {
            try
            {
                if(idPlaca != null)
                {
                    _repositoryPlaca.DeletarPlaca(idPlaca);
                    return Task.CompletedTask;
                }
                throw new Exception("Informe a placa");
            }
            catch (Exception ex)
            {
                Log.Information("Erro ao excluir placa", ex);
                throw new Exception("Não foi possível remover esta placa.");
            }
        }
        public bool AtualizaPlaca(UpdatePlacaDto updatePlaca)
        {
            if (_repositoryPlaca.AtualizaPlaca(updatePlaca))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
