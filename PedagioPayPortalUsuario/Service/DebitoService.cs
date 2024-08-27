using PedagioPayPortalUsuario.Data.Dtos.Debitos;
using PedagioPayPortalUsuario.Data.Interfaces;
using PedagioPayPortalUsuario.Models;
using PedagioPayPortalUsuario.Service.Interfaces;
using Serilog;

namespace PedagioPayPortalUsuario.Service
{
    public class DebitoService : IDebitoService
    {
        private readonly IRepositoryDebito _repositoryDebitos;
        private readonly IConfiguration _configuration;

        public DebitoService(IRepositoryDebito repositoryDebitos, IConfiguration configuration)
        {
            _repositoryDebitos = repositoryDebitos;
            _configuration = configuration;
        }

        public Task AdicionaDebitos(AddDebitosDto debitos)
        {
            try
            {
                DEBITO novoDebito = new DEBITO
                {
                    TOKEN_USUARIO = debitos.TOKEN_USUARIO,
                    ID_PASSAGEM = debitos.ID_PASSAGEM,
                    ID_OSA = debitos.ID_OSA,
                    CATEGORIA = debitos.CATEGORIA,
                    EIXOS = debitos.EIXOS,
                    VALOR = debitos.VALOR,
                    VENCIMENTO = debitos.VENCIMENTO,
                    PAGO_EM = debitos.PAGO_EM,
                    DH_PASSAGEM = debitos.DH_PASSAGEM,
                    CONCESSAO = debitos.CONCESSAO,
                    PLACA = debitos.PLACA,
                    TAG = debitos.TAG,
                };
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível atualizar seus débitos.", ex);
            }
        }

        public List<DebitosDto> BuscaDebitos(string tokenConcessao, string token)
        {
            try
            {
                if (token != null && tokenConcessao != null)
                {
                    if (tokenConcessao == "3")
                    {
                        tokenConcessao = _configuration.GetSection("TokenEconoroeste").Value;
                        var debitosUsuario = _repositoryDebitos.Buscar(tokenConcessao, token);
                        return debitosUsuario.ToList();
                    }
                    if (tokenConcessao == "2")
                    {
                        tokenConcessao = _configuration.GetSection("TokenEcoponte").Value;
                        var debitosUsuario = _repositoryDebitos.Buscar(tokenConcessao, token);
                        return debitosUsuario.ToList();
                    }
                    if (tokenConcessao == "1")
                    {
                        tokenConcessao = null;
                        var debitosUsuario = _repositoryDebitos.Buscar(tokenConcessao, token);
                        return debitosUsuario.ToList();
                    }
                    throw new Exception("Ocorreu um erro ao buscar seus débitos. Tente novamente mais tarde.");
                }
                return null;
            }
            catch (Exception ex)
            {
                Log.Information("Erro ao buscar debitos", ex);
                throw new Exception("Ocorreu um erro ao buscar seus débitos. Tente novamente mais tarde.");
            }
        }

        public List<DebitosDto> HistoricoDebitos(string tokenConcessao, string token)
        {
            try
            {
                if (token != null)
                {
                    if (tokenConcessao == "3")
                    {
                        tokenConcessao = _configuration.GetSection("TokenEconoroeste").Value;
                        var debitosUsuario = _repositoryDebitos.HistoricoDebitos(tokenConcessao, token);
                        return debitosUsuario.ToList();
                    }
                    if (tokenConcessao == "2")
                    {
                        tokenConcessao = _configuration.GetSection("TokenEcoponte").Value;
                        var debitosUsuario = _repositoryDebitos.HistoricoDebitos(tokenConcessao, token);
                        return debitosUsuario.ToList();
                    }
                    if (tokenConcessao == "1")
                    {
                        tokenConcessao = null;
                        var debitosUsuario = _repositoryDebitos.HistoricoDebitos(tokenConcessao, token);
                        return debitosUsuario.ToList();
                    }
                    throw new Exception("Ocorreu um erro ao buscar seus débitos. Tente novamente mais tarde.");

                }
                return null;
            }
            catch (Exception ex)
            {
                Log.Information("Erro ao buscar histórico", ex);
                throw new Exception("Ocorreu um erro ao buscar seu histórico de débitos. Tente novamente mais tarde.");
            }
        }

        public List<DebitosDto> ConsultaAvulsa(string tokenConcessao, ConsultaAvulsaDto consulta)
        {
            try
            {
                if (consulta != null)
                {
                    var teste = consulta.DhFinal;
                    if (tokenConcessao == "3")
                    {
                        tokenConcessao = _configuration.GetSection("TokenEconoroeste").Value;
                        var debitosUsuario = _repositoryDebitos.ConsultaAvulsa(tokenConcessao, consulta);
                        return debitosUsuario.ToList();
                    }
                    if (tokenConcessao == "2")
                    {
                        tokenConcessao = _configuration.GetSection("TokenEcoponte").Value;
                        var debitosUsuario = _repositoryDebitos.ConsultaAvulsa(tokenConcessao, consulta);
                        return debitosUsuario.ToList();
                    }
                    if (tokenConcessao == "1")
                    {
                        tokenConcessao = null;
                        var debitosUsuario = _repositoryDebitos.ConsultaAvulsa(tokenConcessao, consulta);
                        return debitosUsuario.ToList();
                    }
                    throw new Exception("Ocorreu um erro ao buscar seu histórico de débitos. Tente novamente mais tarde.");
                }
                return null;
            }
            catch (Exception ex)
            {
                Log.Information("Erro ao buscar debitos", ex);
                throw new Exception("Ocorreu um erro ao buscar seus débitos. Tente novamente mais tarde.");
            }

        }
    }
}
