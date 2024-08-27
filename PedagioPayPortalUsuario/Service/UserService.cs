using PedagioPayFadamiBack.Data.Dto;
using PedagioPayFadamiBack.Data.Repository.Interface;
using PedagioPayFadamiBack.Service.Interface;
using BCrypt;
using PedagioPayPortalUsuario.Models;
using PedagioPayPortalUsuario.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;
namespace PedagioPayFadamiBack.Service;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserPlacaRepository _userPlacaRepository;
    private readonly ITokenService _tokenService;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;
    public UserService(IUserRepository userRepository, ITokenService tokenService,
        IEmailService emailService, IConfiguration configuration,
        IUserPlacaRepository userPlacaRepository) 
    { 
        _userRepository = userRepository;
        _tokenService = tokenService;
        _emailService = emailService;
        _configuration = configuration;
        _userPlacaRepository = userPlacaRepository;
    }
    public async Task<UserDto> Cadastra(string tokenConcessao, CadastroDto novoUsuario)
    {
        string CodValidacao = _emailService.CriaCodigoConfirmacao();

        try
        {
            var hashSenha = EncryptSenha(novoUsuario.Senha);
            string token = _tokenService.GenerateToken(novoUsuario.Email, _configuration.GetSection("DevAppSettings:Token").Value, 8);

            USUARIO usuario = new USUARIO()
            {
                NOME = novoUsuario.Nome,
                EMAIL = novoUsuario.Email,
                CPF_CNPJ = novoUsuario.Cpf_Cnpj,
                SENHA = hashSenha.ToString(),
                CELULAR = novoUsuario.Celular,
                CODIGO_VALIDACAO = CodValidacao,
                BL_VALIDADO = false,
                TOKEN = token
            };

            PLACAs placa = new PLACAs
            {
                PLACA = novoUsuario.Placa,
                MODELO = novoUsuario.Marca_Modelo,
                TOKEN_USUARIO = token,
                USUARIO_EMAIL = novoUsuario.Email
            };

            //ADICIONA USUÁRIO NA BASE DE DADOS.
            var adicionaNovoUsuario = await _userRepository.Cadastra(usuario);
            if (adicionaNovoUsuario != null)
            {
                //ADICIONA PLACA NA BASE DE DADOS.
                var adicionaPlaca = _userPlacaRepository.Cadastra(placa);

                if (adicionaNovoUsuario != null && adicionaPlaca != null)
                {
                    //ENVIA EMAIL COM CÓDIGO DE CONFIRMAÇÃO DE CADASTRO
                    var newBody = _emailService.ConfirmRegisterBody(tokenConcessao, novoUsuario.Nome, usuario.CODIGO_VALIDACAO);
                   await _emailService.NewEmail(novoUsuario.Email, newBody, "Confirmação de Cadastro");
                   return adicionaNovoUsuario;
                }
                throw new Exception("Cadastro não realizado.");
            }
            throw new Exception("Cadastro não realizado.");
        }
        catch (Exception ex) 
        {
            throw new Exception("Cadastro não realizado.");
        }
    }
    
    public async Task<UserDto> Autentica(AutenticacaoDto login)
    {
        try
        {
            var logar = await _userRepository.Autentica(login);
            if (!BCrypt.Net.BCrypt.Verify(login.Senha, logar.Senha))
            {
                Log.Error("erro ao autenticar 1");
                if (!BCrypt.Net.BCrypt.EnhancedVerify(login.Senha, logar.Senha))
                {
                    Log.Error("erro ao autenticar 2");
                    throw new Exception();
                }
                return logar;
            }
            return logar;
        }catch (Exception ex)
        {
            throw new Exception();
        }
    }
    public bool ConfirmacaoCadastro(string tokenValidacao)
    {
        try
        {
           if(_userRepository.confirmacaoCadastro(tokenValidacao))
           {
                return true;
           }
            throw new Exception("Verique seu endereço de email.");
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao confirmar cadastro.");
        }
    }

    public string ReenviaTokenValidacao(string tokenConcessao, string userEmail, string tokenUsuario)
    {
        try
        {
            string userCodigo = _userRepository.ReenviaTokenValidacao(userEmail,tokenUsuario);
            if (userCodigo != null)
            {
                var newBody = _emailService.ConfirmRegisterBody(tokenConcessao,"", userCodigo);
                 _emailService.NewEmail(userEmail, newBody, "Confirmação de Cadastro");
                return userCodigo;
            }
            throw new Exception("Código não encontrado.");
        }catch (Exception ex)
        {
            throw new Exception("Erro ao reenviar código.");
        }
    }
    public string EncryptSenha(string senha)
    {
        var senhaHash = BCrypt.Net.BCrypt.HashPassword(senha, 13);
        return senhaHash;
    }
}
