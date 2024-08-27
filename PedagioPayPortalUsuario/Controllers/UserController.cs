using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PedagioPayFadamiBack.Data.Dto;
using PedagioPayFadamiBack.Data.Repository.Interface;
using PedagioPayFadamiBack.Service.Interface;
using PedagioPayPortalUsuario.Models;
using PedagioPayPortalUsuario.Service.Interfaces;
using System.Text.Json;

namespace PedagioPayPortalUsuario.Controllers;
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly PEDAGIOPAYContext _context;
    private readonly IUserRepository _userRepository;
    private readonly IUserPlacaRepository _userPlacaRepository;
    private readonly ITokenService _tokenService;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;
    public UserController(IUserService userService, PEDAGIOPAYContext context,
        IUserRepository userRepository, ITokenService tokenService,
        IEmailService emailService, IConfiguration configuration,
        IUserPlacaRepository userPlacaRepository)
    {
        _context = context;
        _userService = userService;
        _userRepository = userRepository;
        _tokenService = tokenService;
        _emailService = emailService;
        _configuration = configuration;
        _userPlacaRepository = userPlacaRepository;
    }
    [HttpGet("health")]
    public async Task<IActionResult> Health()
    {
        try
        {
            var retorno = new
            {
                status = "ok",
                date = DateTime.Now
            };

            var content = JsonSerializer.Serialize(retorno);

            HttpContext.Response.ContentLength = content.Length;
            HttpContext.Response.Headers.ContentLength = content.Length;

            return Ok(retorno);
        }
        catch (Exception ex)
        {
            return BadRequest();
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult> CadastroInicial(CadastroDto novoUsuario)
    {
        string CodValidacao = _emailService.CriaCodigoConfirmacao();

        try
        {
            var hashSenha = _userService.EncryptSenha(novoUsuario.Senha);
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
                    var newBody = _emailService.ConfirmRegisterBody(novoUsuario.TokenConcessao, novoUsuario.Nome, usuario.CODIGO_VALIDACAO);
                    await _emailService.NewEmail(novoUsuario.Email, newBody, "Confirmação de Cadastro");
                    //return adicionaNovoUsuario;
                    return Ok(new { code = StatusCode(200), token = adicionaNovoUsuario.Token, nome = adicionaNovoUsuario.Nome, cpf_cnpj = adicionaNovoUsuario.Cpf_Cnpj, email = adicionaNovoUsuario.Email, telefone = adicionaNovoUsuario.Celular, placa = adicionaPlaca.PLACA});
                }
                return BadRequest(new { code = StatusCode(401), menage = "Não foi possível enviar o token de confirmação. Tente novamente"});
            }
            if(adicionaNovoUsuario == null)
            {
                return BadRequest(new { code = StatusCode(401), mensage = "Usuário já cadastrado." });
            }
            return BadRequest(new { code = StatusCode(400), mensage = "Preencha todos os dados." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { code = StatusCode(400), mensage = "Não foi possível rezalizar o cadastro." });
        }

    }

    [HttpPost("auth")]
    public async Task<ActionResult> Autenticacao(AutenticacaoDto request)
    {
        try
        {
            var novaSessao = await _userService.Autentica(request);
            if (novaSessao == null)
            {
                return BadRequest(new { code = StatusCode(401), mensage = "Usuário ou senha inválidos." });
            }
            return Ok(new { code = StatusCode(200), token = novaSessao.Token, nome = novaSessao.Nome, cpf_cnpj = novaSessao.Cpf_Cnpj, email = novaSessao.Email, telefone = novaSessao.Celular });
        }
        catch (Exception ex)
        {
            return BadRequest(new { code = StatusCode(400), mensage = "Usuário inexistente ou não validado." });
        }
    }

    [HttpPost("sendTokenValidation/{tokenConcessao}")]
    public ActionResult ReenviaTokenValidacao([FromRoute] string tokenConcessao, [FromBody] string userEmail, [FromHeader] string tokenUsuario)
    {
        try
        {
            var sendToken = _userService.ReenviaTokenValidacao(tokenConcessao, userEmail, tokenUsuario);
            if (sendToken != null)
            {
                return Ok(new { code = StatusCode(200), mensagem = "Token reenviado." });
            }
            return BadRequest(new { code = StatusCode(401), mensage = "Não foi possível reenviar o token." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { code = StatusCode(400), mensage = "Não foi possível reenviar o token. " });
        }
    }

    [HttpPost("confirmRegister")]
    public ActionResult ConfirmacaoCadastro([FromHeader] string tokenValidacao)
    {
        try
        {
            if (_userService.ConfirmacaoCadastro(tokenValidacao))
            {
                return Ok(new { code = StatusCode(200) });
            }
            return BadRequest(new { code = StatusCode(401), mensage = "Token inválido." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { code = StatusCode(400), mensage = "Não foi possível validar cadastro." });
        }
    }
}
    /*[HttpGet("MinhaConta")]
    public Action*/

