using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Crypto.Generators;
using PedagioPayFadamiBack.Data.Dto;
using PedagioPayPortalUsuario.Data.Dtos;
using PedagioPayPortalUsuario.Data.Dtos.Usuario;
using PedagioPayPortalUsuario.Data.Interfaces;
using PedagioPayPortalUsuario.Models;
using PedagioPayPortalUsuario.Service.Interfaces;
using Serilog;
using System.Security.Cryptography;

namespace PedagioPayPortalUsuario.Service
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IRepositoryUsuario _repositoryUsuario;

        public UsuarioService(IRepositoryUsuario repositoryUsuario)
        {
            _repositoryUsuario = repositoryUsuario;
        }

        public Task Cadastra(USUARIO usuario)
        {
            try
            {
                return _repositoryUsuario.Cadastra(usuario);
            }
            catch (Exception ex)
            {
                Log.Information("Erro ao cadastrar", ex);
                throw new Exception("Usuario não encontrado.", ex);
            }
        }

        public Task CadastraExterno(USUARIO usuarioExterno)
        {
            try
            {
                _repositoryUsuario.CadastraExterno(usuarioExterno);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Log.Information("Erro ao cadastrar usuario externo.", ex);
                throw new Exception("Usuario não encontrado.", ex);
            }
        }

        public Task Atualiza(UpdateUsuarioDto usuario) 
        {
            try
            {
                _repositoryUsuario.Atualiza(usuario);
                return Task.CompletedTask;
            }catch (Exception ex)
            {
                Log.Information("Erro ao cadastrar usuario externo.", ex);
                throw new Exception("Verique se todos campos foram preenchidos corretamente.", ex);
            }
        }

        public bool BuscaUsuario(string usuario, string campo, string token)
        {
            try
            {
                if (_repositoryUsuario.Autentica(usuario, campo, token))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Log.Information("Erro ao buscar usuario.", ex);
                return false;
            }
        }

        public bool VerificaCadastro(string login)
        {
            try
            {
                var usuario = _repositoryUsuario.Busca(login);
                if (usuario == null)
                {
                    return false;
                }

                return true;

            }catch (Exception ex) {
                Log.Information("Erro ao verificar cadastro.", ex);
                return false;
            }
        }

        public bool AtualizaSenha(string novaSenha, int tokenUsuario, string emailUsuario)
        {
            try
            {

                var hashNovaSenha = EncryptSenha(novaSenha);
                if (_repositoryUsuario.AtualizaSenha(hashNovaSenha, tokenUsuario, emailUsuario))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Log.Information("Erro ao atualizar senha.", ex);
                return false;
            }

        }

        public bool VerificaEmail(string email)
        {
            try
            {
                if (!_repositoryUsuario.VerificaEmail(email))
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Information("Erro ao atualizar senha.", ex);
                return false;
            }

        }

        public bool VerificaSenha(string login, string campo, string senha)
        {
            try
            {
                var hash = _repositoryUsuario.BuscaSenha(login, campo);
                if (BCrypt.Net.BCrypt.EnhancedVerify(senha, hash))
                {
                    return true;
                }
                return false;
            }
            catch(Exception ex) 
            {
                Log.Information("Erro ao verificar senha.", ex);
                return false;
            }
        }

        public bool VerificaAtivo(string login, string campo, string token)
        {
            try
            {
                if (_repositoryUsuario.VerificaAtivo(login, campo, token))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Log.Information("Erro ao verificar senha.", ex);
                return false;
            }
        }

        public string TokenUsuario(string email, string campo)
        {
            try
            {
                return _repositoryUsuario.TokenUsuario(email, campo);
            }
            catch (Exception ex)
            {
                Log.Information("Erro ao verificar senha.", ex);
                throw new Exception("", ex);
            }
        }

        public string GetTokenValidate(string email)
        {
            try
            {
                return _repositoryUsuario.GetTokenValidate(email);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public string EncryptSenha(string senha)
        {
            var senhaHash =  BCrypt.Net.BCrypt.EnhancedHashPassword(senha, 13);
            return senhaHash;
        }

        public UserDto GetUsuario(string token)
        {
            return _repositoryUsuario.GetUsuario(token);
        }

        public int BuscarIdUsuarioPorEmail(string email)
        {
            try
            {
                return _repositoryUsuario.BuscarIdUsuarioPorEmail(email);
            }
            catch (Exception ex)
            {
                Log.Information("Erro ao buscar id do usuario.", ex);
                throw new Exception("Erro ao buscar id do usuario.", ex);
            }
        }
    }
}
