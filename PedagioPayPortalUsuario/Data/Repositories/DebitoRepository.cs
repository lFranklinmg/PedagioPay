using Dapper;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using PedagioPayPortalUsuario.Data.Dtos;
using PedagioPayPortalUsuario.Data.Dtos.Debitos;
using PedagioPayPortalUsuario.Data.Interfaces;
using PedagioPayPortalUsuario.Models;
using Serilog;
using System.Data;
using System.Numerics;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Superpower.Model;


namespace PedagioPayPortalUsuario.Data.Repositories
{
    public class DebitoRepository : IRepositoryDebito
    {
        private readonly PEDAGIOPAYContext _context;
        private readonly IConfiguration _configuration;


        public DebitoRepository(PEDAGIOPAYContext context, IConfiguration configuration)
        {
            _context = context;
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            _configuration = configuration;

        }

        public Task Cadastra(DEBITO usuarioDebitos)
        {
            //Descontinuado
            try
            {
                var consultaUsuario = _context.PLACAs
                    .Where(x => x.PLACA == usuarioDebitos.PLACA)
                    .Select(e => e.USUARIO_EMAIL)
                    .FirstOrDefault();
                var consultaToken = _context.USUARIOs
                    .Where(x => x.EMAIL == consultaUsuario)
                    .Select(e => e.TOKEN)
                    .FirstOrDefault();
                usuarioDebitos.TOKEN_USUARIO = consultaToken;
                _context.Add(usuarioDebitos);
                _context.SaveChanges();
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Log.Information( "erro ao cadastrar debitos no banco",ex);
                throw new Exception("Erro ao adicionar débitos.", ex);
            }
        }

        public List<DebitosDto> Buscar(string tokenConcessao, string token)
        {
            try
            {
                using (var _dbConnection = new SqlConnection(_configuration.GetConnectionString("DevelopmentDB")))
                    return _dbConnection.Query<DebitosDto>(
                        "SPC_DEBITO",
                        new
                        {
                            TOKEN_CONCESSAO = tokenConcessao,
                            TOKEN = token
                        },
                        commandType: CommandType.StoredProcedure
                    ).ToList();
            }
            catch (Exception ex)
            {
                Log.Information("erro ao buscar debitos no banco", ex);
                throw new Exception("Não foi possível encontrar seus débitos.");
            }
        }

        public List<DebitosDto> HistoricoDebitos(string tokenConcessao, string token)
        {
            try
            {
                using (var _dbConnection = new SqlConnection(_configuration.GetConnectionString("DevelopmentDB")))
                    return _dbConnection.Query<DebitosDto>(
                        "SPC_HISTORICO",
                        new
                        {
                            TOKEN_CONCESSAO = tokenConcessao,
                            TOKEN = token
                        },
                        commandType: CommandType.StoredProcedure
                    ).ToList();
            }
            catch (Exception ex)
            {
                Log.Information("erro ao buscar histórico no banco", ex);
                throw new Exception("Não foi possível encontrar seus débitos.");
            }
        }

        public List<DebitosDto> ConsultaAvulsa(string tokenConcessao, ConsultaAvulsaDto consulta)
        {
            try
            {
                var testeI = consulta.DhInicio;
                var TestII = consulta.DhFinal;
                using (var _dbConnection = new SqlConnection(_configuration.GetConnectionString("DevelopmentDB")))
                    return _dbConnection.Query<DebitosDto>(
                        "SPC_PASSAGEM_SEM_CADASTRO",
                        new
                        {
                            TOKEN_CONCESSAO = tokenConcessao,
                            PLACA = consulta.Placa,
                            DH_INICIO = consulta.DhInicio.Date,
                            DH_FIM = consulta.DhFinal.Date,
                        },
                        commandType: CommandType.StoredProcedure
                    ).ToList();
            }
            catch
            {
                throw new Exception("Não foi possível encontrar seus débitos.");

            }
        }
    }
}
