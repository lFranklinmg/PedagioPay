using Dapper;
using Fadami.Helper.Emails.Entity;
using Fadami.Helper.Logs;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PedagioPayFadamiBack.Data.Dto;
using PedagioPayPortalUsuario.Data.Dtos.Debitos;
using PedagioPayPortalUsuario.Data.Dtos.Usuario;
using PedagioPayPortalUsuario.Data.Interfaces;
using PedagioPayPortalUsuario.Models;
using Serilog;
using System.Data;
using System.Globalization;
using System.Linq.Expressions;
namespace PedagioPayPortalUsuario.Data.Repositories
{
    public class UsuarioRepository : IRepositoryUsuario
    {
        private readonly PEDAGIOPAYContext _context;
        private readonly IConfiguration _configuration;
        public UsuarioRepository(PEDAGIOPAYContext context, IConfiguration configuration)
        {
            _context = context;
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            _configuration = configuration;
        }
        public Task Cadastra(USUARIO usuario)
        {
            try
            {
                _context.Add(usuario);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Serilog.Log.Information("Erro ao cadastrar no banco", ex);
                throw new Exception("Verique se todos campos foram preenchidos corretamente.", ex);
            }
            return Task.CompletedTask;
        }

        public Task CadastraExterno(USUARIO usuarioExterno)
        {
            try
            {
                _context.Add(usuarioExterno);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Serilog.Log.Information("Erro ao cadastrar conta externa no banco", ex);
                throw new Exception("Erro ao conectar à conta externa", ex);
            }
            return Task.CompletedTask;
        }

        public Task Atualiza(UpdateUsuarioDto dados)
        {
            var usuario = _context.USUARIOs.FirstOrDefault(x => x.EMAIL == dados.EMAIL);
            if (usuario != null)
            {
                try
                {
                    usuario.CELULAR = dados.CELULAR;
                    usuario.CPF_CNPJ = dados.CPF_CNPJ;
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Serilog.Log.Information("Erro ao atualizar dados no banco", ex);
                    throw new Exception("Erro ao atualizar dados.", ex);
                }
            }

            return Task.CompletedTask;
        }


        public bool AtualizaSenha(string novaSenha, int tokenUsuario, string emailUsuario)
        {
            try
            {
                var usuarioEmail = _context.USUARIOs.Where(u => u.EMAIL == emailUsuario).FirstOrDefault();
                if (usuarioEmail != null)
                {
                    if (usuarioEmail.ID_USUARIO == tokenUsuario)
                    {
                        usuarioEmail.SENHA = novaSenha;
                        _context.SaveChanges();
                        return true;
                    }
                    throw new Exception("Identificador não conscide.");
                }
                return false;
            }
            catch (Exception ex)
            {
                Serilog.Log.Information("Erro ao atualizar senha no banco", ex);
                throw new Exception("Erro ao alterar senha.", ex);
            }
            return false;
        }

        public bool Autentica(string login, string campoBusca, string token)
        {
            try
            {
                var entityType = typeof(USUARIO);
                var property = entityType.GetProperty(campoBusca);
                if (property == null)
                {
                    return false;
                }
                var parameter = Expression.Parameter(entityType, "x");
                var body = Expression.Equal(Expression.Property(parameter, property), Expression.Constant(login));
                var lambda = Expression.Lambda<Func<USUARIO, bool>>(body, parameter);
                var usuario = _context.Set<USUARIO>().Where(lambda).FirstOrDefaultAsync().GetAwaiter().GetResult();
                // var debitos = _context.DEBITOs.FirstOrDefault(x => x.U == email);

                if (usuario != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Serilog.Log.Information("Erro ao autenticar no banco", ex);
                throw new Exception("Usuário ou senha inválidos", ex);
            }
        }

        public async Task<USUARIO> Busca(string login)
        {
            try
            {
                if (login != null)
                {
                    var usuario = _context.Set<USUARIO>().FirstOrDefault(x => x.TOKEN == login);
                    if (usuario.CPF_CNPJ == null)
                    {
                        return null;
                    }
                    return usuario;
                }

                return null;
            }
            catch (Exception ex)
            {
                Serilog.Log.Information("Erro ao buscar no banco", ex);
                throw new Exception("Erro ao buscar usuário", ex);
            }
        }

        public bool VerificaEmail(string emailUsuario)
        {
            try
            {
                var buscaEmail = _context.Set<USUARIO>().FirstOrDefault(x => x.EMAIL == emailUsuario);
                if (buscaEmail != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Serilog.Log.Information("Erro ao autenticar no banco", ex);
                throw;
            }
        }

        public bool VerificaAtivo(string login, string campo, string token)
        {
            try
            {
                var entityType = typeof(USUARIO);
                var property = entityType.GetProperty(campo);
                var parameter = Expression.Parameter(entityType, "x");
                var body = Expression.Equal(Expression.Property(parameter, property), Expression.Constant(login));
                var lambda = Expression.Lambda<Func<USUARIO, bool>>(body, parameter);
                var usuario = _context.Set<USUARIO>().Where(lambda).FirstOrDefaultAsync().GetAwaiter().GetResult();

                if (usuario.CPF_CNPJ != null)
                {
                    if (usuario.BL_VALIDADO != false)
                    {
                        if (usuario.EMAIL != null || usuario.ID_FACEBOOK != null || usuario.ID_GOOGLE != null)
                        {

                            return true;
                        }
                        return false;
                    }
                    return false;
                }
                return false;
            }
            catch (Exception ex)
            {
                Serilog.Log.Information("Erro ao verificar ativo", ex);
                throw;
            }
        }

        public string BuscaSenha(string login, string campo)
        {
            try
            {
                var entityType = typeof(USUARIO);
                var property = entityType.GetProperty(campo);
                var parameter = Expression.Parameter(entityType, "x");
                var body = Expression.Equal(Expression.Property(parameter, property), Expression.Constant(login));
                var lambda = Expression.Lambda<Func<USUARIO, bool>>(body, parameter);
                var usuario = _context.Set<USUARIO>().FirstOrDefaultAsync(lambda).GetAwaiter().GetResult();

                if (usuario != null)
                {
                    return usuario.SENHA;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Information("Erro ao buscar senha no banco", ex);
                throw;
            }
        }

        public bool BuscaCodConfirmacao(string codigo)
        {
            try
            {
                var usuario = _context.Set<USUARIO>().FirstOrDefault(x => x.CODIGO_VALIDACAO == codigo);

                if (usuario != null)
                {
                    usuario.BL_VALIDADO = true;
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                // Log.Information("Erro ao buscar codigo confirmacao no banco", ex);
                throw new Exception("Erro ao buscar código de confirmação.", ex);
            }
        }

        public string TokenUsuario(string email, string campo)
        {
            try
            {
                using (var _dbConnection = new SqlConnection(_configuration.GetConnectionString("DevelopmentDB")))
                    return _dbConnection.ExecuteScalar(
                        "SPC_TOKEN_USUARIO",
                        new
                        {
                            EMAIL = email,
                            CPF_CNPJ = email,
                            CAMPO = campo
                        },
                        commandType: CommandType.StoredProcedure
                    ).ToString();

                /*var entityType = typeof(USUARIO);
                var property = entityType.GetProperty(campo);
                var parameter = Expression.Parameter(entityType, "x");
                var body = Expression.Equal(Expression.Property(parameter, property), Expression.Constant(email));
                var lambda = Expression.Lambda<Func<USUARIO, bool>>(body, parameter);
                var token = _context.USUARIOs.Where(lambda).Select(u => u.TOKEN.FirstOrDefault());

                return token.ToString();*/
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar token", ex);
            }
        }

        public string GetTokenValidate(string email)
        {
            try
            {
                var usuario = _context.USUARIOs.Where(u => u.EMAIL == email).FirstOrDefault();
                if (usuario != null)
                {
                    return usuario.CODIGO_VALIDACAO;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public string GetTokenUsuario(string email)
        {
            try
            {
                var usuario = _context.USUARIOs.Where(u => u.EMAIL == email).FirstOrDefault();
                return usuario.EMAIL;
            }
            catch
            {
                throw new Exception();
            }
        }

        public UserDto GetUsuario(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    throw new ArgumentNullException(nameof(token), "Token não pode ser nulo ou vazio.");
                }

                var usuario = _context.USUARIOs.FirstOrDefault(u => u.TOKEN == token);

                if (usuario == null)
                {
                    return null;
                }

                var usuarioDTO = new UserDto
                {
                    Nome = usuario.NOME,
                    Email = usuario.EMAIL,
                    Cpf_Cnpj = usuario.CPF_CNPJ,
                    Celular = usuario.CELULAR,
                    Id = usuario.ID_USUARIO
                };

                return usuarioDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao buscar o usuário.", ex);
            }
        }

        public int BuscarIdUsuarioPorEmail(string emailUsuario)
        {
            try
            {
                var usuario = _context.USUARIOs.Where(u => u.EMAIL == emailUsuario).FirstOrDefault();
                return usuario.ID_USUARIO;
            }
            catch
            {
                throw new Exception();
            }
        }
    }
}
