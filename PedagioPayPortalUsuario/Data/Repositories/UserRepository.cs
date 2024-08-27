using Fadami.Helper.Emails.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using PedagioPayFadamiBack.Data.Dto;
using PedagioPayFadamiBack.Data.Repository.Interface;
using PedagioPayPortalUsuario.Models;
using PedagioPayPortalUsuario.Service.Interfaces;
using System.Linq.Expressions;

namespace PedagioPayFadamiBack.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly PEDAGIOPAYContext _context;
        private readonly IConfiguration _configuration;
        public UserRepository(IConfiguration configuration, PEDAGIOPAYContext context
    )
        {
            _configuration = configuration;
            _context = context;
        }
        public async Task<UserDto> Cadastra(USUARIO novoUsuario)
        {
            try
            {
                var email = _context.USUARIOs.Where(x => x.EMAIL == novoUsuario.EMAIL).FirstOrDefault();
                var cpf = _context.USUARIOs.Where(x => x.CPF_CNPJ == novoUsuario.CPF_CNPJ).FirstOrDefault();
                if (email == null && cpf == null)
                {
                    _context.Add(novoUsuario);
                    _context.SaveChanges();

                    var retorno = new UserDto()
                    {
                        Nome = novoUsuario.NOME,
                        Email = novoUsuario.EMAIL,
                        Token = novoUsuario.TOKEN
                    };
                    return retorno;
                }
                else
                {
                    return null;
                }
                throw new Exception("Erro adicionar usuário na base.");
            }
            catch (Exception ex)
            {
                throw new Exception("Erro adicionar usuário na base.", ex);
            }
        }
        public async Task<UserDto> Autentica(AutenticacaoDto login)
        {
            try 
            {
                var entityType = typeof(USUARIO);
                var property = entityType.GetProperty(login.TipoLogin);
                var parameter = Expression.Parameter(entityType, "x");
                var body = Expression.Equal(Expression.Property(parameter, property), Expression.Constant(login.Login));
                var lambda = Expression.Lambda<Func<USUARIO, bool>>(body, parameter);
                var usuario = _context.Set<USUARIO>().Where(lambda).FirstOrDefaultAsync().GetAwaiter().GetResult();
                if (usuario.BL_VALIDADO != false)
                {
                    var retorno = new UserDto()
                    {
                        Nome = usuario.NOME,
                        Email = usuario.EMAIL,
                        Cpf_Cnpj = usuario.CPF_CNPJ,
                        Senha = usuario.SENHA,
                        Token = usuario.TOKEN
                    };
                    return retorno;
                }
                throw new Exception("Informações inválidas.");
            }catch (Exception ex)
            {
                throw new Exception("Não autenticado.", ex);
            }
        }
        public bool confirmacaoCadastro(string tokenValidacao)
        {
            try
            {
                var usuario = _context.Set<USUARIO>().FirstOrDefault(x => x.CODIGO_VALIDACAO == tokenValidacao);
                if(usuario != null)
                {
                    usuario.BL_VALIDADO = true;
                    _context.SaveChanges();
                    return true;
                }
                throw new Exception("Código não encontrado.");
            }
            catch (Exception ex) 
            {
                throw new Exception("Erro ao buscar token de validação.", ex);
            }
        } 
        public string ReenviaTokenValidacao(string userEmail, string tokenUsuario)
        {
            try
            {
                var usuario = _context.USUARIOs.Where(u => u.TOKEN == tokenUsuario).FirstOrDefault();
                //var usuario = _context.Set<USUARIO>().FirstOrDefault(x => x.EMAIL == userEmail);
                if (usuario.EMAIL != userEmail)
                {
                    usuario.EMAIL = userEmail;
                    _context.SaveChanges();
                    return usuario.CODIGO_VALIDACAO;
                }
                return usuario.CODIGO_VALIDACAO;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar token de validação.", ex);
            }
        }
    }
}
