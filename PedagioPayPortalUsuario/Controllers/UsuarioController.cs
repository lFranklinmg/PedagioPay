using Serilog;
using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Asn1.Ocsp;
using PedagioPayPortalUsuario.Data;
using PedagioPayPortalUsuario.Data.Dtos;
using PedagioPayPortalUsuario.Data.Dtos.AutenticacaoExterna;
using PedagioPayPortalUsuario.Data.Dtos.Email;
using PedagioPayPortalUsuario.Data.Dtos.Usuario;
using PedagioPayPortalUsuario.Models;
using PedagioPayPortalUsuario.Service;
using PedagioPayPortalUsuario.Service.Interfaces;
using System;
using System.Text.Json;
using PedagioPayPortalUsuario.Models.Util;
using Fadami.Helper.Emails;
using Microsoft.Extensions.Primitives;
using PedagioPayFadamiBack.Data.Repository.Interface;
using PedagioPayPortalUsuario.Data.Interfaces;

namespace PedagioPayPortalUsuario.Controllers;

[ApiController]
[Route("[controller]")]
public class UsuarioController : ControllerBase
{
    private PEDAGIOPAYContext _context;
    private IMapper _mapper;
    private readonly IUsuarioService _usuarioService;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;
    private readonly ITokenService _tokenService;
    private readonly IAutenticacaoExterna _autenticacaoExternaService;
    private static readonly HttpClient HttpClient = new HttpClient();
    private readonly IRepositoryUsuario _repositoryUsuario;


    public UsuarioController(PEDAGIOPAYContext context,
        IMapper mapper,
        IUsuarioService usuarioService,
        IConfiguration configuration,
        IEmailService emailService,
        ITokenService tokenService,
        IAutenticacaoExterna autenticacaoExternaService, 
        IRepositoryUsuario repositoryUsuario)
    {
        _context = context;
        _mapper = mapper;
        _usuarioService = usuarioService;
        _configuration = configuration;
        _emailService = emailService;
        _tokenService = tokenService;
        _autenticacaoExternaService = autenticacaoExternaService;
        _usuarioService = usuarioService;
    }

    [HttpPost("Cadastro")]
    public async Task<ActionResult<USUARIO>> CadastraUsuario(CreateUsuarioDto request)
    { 

        if (!_usuarioService.VerificaEmail(request.EMAIL))
        {
            var hashSenha = _usuarioService.EncryptSenha(request.SENHA);
            string token = _tokenService.GenerateToken(request.EMAIL, _configuration.GetSection("DevAppSettings:Token").Value, 8);
            hashSenha.ToString();
            var emailBody = _emailService.CriaCodigoConfirmacao();
            USUARIO usuario = new USUARIO()
            {
                NOME = request.NOME,
                EMAIL = request.EMAIL,
                CPF_CNPJ = request.CPF_CNPJ,
                CELULAR = request.CELULAR,
                SENHA = hashSenha.ToString(),
                CODIGO_VALIDACAO = emailBody,
                BL_VALIDADO = request.BL_VALIDADO,
                TOKEN = token
            };


            try
            {
               /*Log.Information("Iniciar envio de email");
                string smtp = _configuration.GetSection("DevAppSettings:DevDeafaultSmtp:Host").Value;
                string username = _configuration.GetSection("DevAppSettings:DevDeafaultSmtp:Auth:User").Value;
                string password = _configuration.GetSection("DevAppSettings:DevDeafaultSmtp:Auth:Password").Value;
                string porta = _configuration.GetSection("DevAppSettings:DevDeafaultSmtp:Port").Value;
                Log.Information($"Email usuario : {usuario.EMAIL}");
                var email = Util.Email("PedagioPay", "Confimação de email | Pedagio Pay", smtp, username, password, porta);
                Log.Information($"Objeto email: {JsonSerializer.Serialize(email)}");
                email.Destinatarios.Add(usuario.EMAIL);
                email.Texto = emailBody;
                email.CorpoHTML = true;

                GerenciarEmail.Enviar(email);*/
                Log.Information("Envio de email realizado com sucesso");
                SmtpDto smtp = new SmtpDto()
                {
                    Host = _configuration.GetSection("DevAppSettings:DevDeafaultSmtp:Host").Value,
                    Port = int.Parse(_configuration.GetSection("DevAppSettings:DevDeafaultSmtp:Port").Value),
                    AuthUser = _configuration.GetSection("DevAppSettings:DevDeafaultSmtp:Auth:User").Value,
                    AuthPass = _configuration.GetSection("DevAppSettings:DevDeafaultSmtp:Auth:Password").Value
                };
                EmailDto email = new EmailDto()
                {
                    To = request.EMAIL,
                    From = _configuration.GetSection("DevAppSettings:DefaultEmail").Value,
                    Body = emailBody,
                    Subject = "Confirmação de cadastro | PedagioPay"
                };
                await _emailService.SendEmail(email, smtp);
                await _usuarioService.Cadastra(usuario);
                return Ok(new {code = StatusCode(201), mensage = "Código de Confirmação enviado."});

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        if (_usuarioService.VerificaEmail(request.EMAIL))
        {
            return BadRequest(new {code = StatusCode(400), mensage = "Usuário já existe."});
        }
        return BadRequest(new { code = StatusCode(400), mensage = "Erro ao realizar cadastro." });
    }

    [HttpPost("ConfirmacaoCadastro")]
    public IActionResult ConfirmacaoCadastro([FromBody] string codRecebido)
    {
        if (_emailService.ValidaCodigoConfirmacao(codRecebido))
        {
            string token = _tokenService.GenerateToken(codRecebido, _configuration.GetSection("DevAppSettings:Token").Value, 8);
            return Ok(new {code = StatusCode(200), mensage = "Cadastro concluído.", token = token});
        }
        return BadRequest();
    }

    [HttpPost("Autenticacao")]
    public async Task<ActionResult<string>> Login(LoginDto request)
    {
        string token = _usuarioService.TokenUsuario(request.Usuario, request.Campo);

        if (_usuarioService.BuscaUsuario(request.Usuario, request.Campo, token) && _usuarioService.VerificaSenha(request.Usuario, request.Campo, request.Senha) && _usuarioService.VerificaAtivo(request.Usuario, request.Campo, token))
        {
            return Ok(new {code = StatusCode(200), token = token.ToString()});
        }
        return BadRequest(new { code = StatusCode(401), mensage = "Usuário ou senha inválidos." });
    }

    [HttpPost("AutenticacaoGoogle")]
    public async Task<ActionResult<USUARIO>> AutenticacaoGoogle(LoginGoogleDto request)
    {
        var url = request.UsuarioID;
        var resultado = await HttpClient.GetAsync(url);
        var retorno = await resultado.Content.ReadAsStringAsync();
        JObject json = JObject.Parse(retorno);

        var usuarioId = json.GetValue("sub").ToString();

        string token = _tokenService.GenerateToken(request.UsuarioID, _configuration.GetSection("DevAppSettings:Token").Value, 8);

        if (_usuarioService.VerificaAtivo(usuarioId, request.Campo, token) && _usuarioService.BuscaUsuario(usuarioId, request.Campo, token))
        {
            return Ok(new { code = StatusCode(200), token = token });
        }
        USUARIO usuario = new USUARIO()
        {
            NOME = json.GetValue("name").ToString(),
            EMAIL = json.GetValue("email").ToString(),
            CPF_CNPJ = "",
            CELULAR = "",
            SENHA = "",
            CODIGO_VALIDACAO = "",
            BL_VALIDADO = true,
            CD_STATUS = true,
            DH_TIMESTAMP = DateTime.UtcNow,
            ID_FACEBOOK = "",
            ID_GOOGLE = json.GetValue("sub").ToString(),
            ID_APPLE = ""
        };
        try
        {
            await _usuarioService.CadastraExterno(usuario);
            return Ok(new { code = StatusCode(200), token = token });
        }
        catch (Exception ex)
        {
            return BadRequest(new { code = StatusCode(400), mensage = "Erro ao conectar a conta externa." });

        }
    }

    [HttpPost("AutenticacaoFacebook")]
    public async Task<ActionResult<USUARIO>> AutenticacaoExterna(LoginFacebookDto facebook)
    {
        string token = _tokenService.GenerateToken(facebook.Id_Usuario, _configuration.GetSection("DevAppSettings:Token").Value, 8);

        if (_usuarioService.VerificaAtivo(facebook.Id_Usuario, facebook.Campo, token))
        {
            return Ok(new { code = StatusCode(200), token = token });
        }
        USUARIO usuario = new USUARIO()
        {
            NOME = facebook.Nome,
            EMAIL = facebook.Email,
            CPF_CNPJ = "",
            CELULAR = "",
            SENHA = "",
            CODIGO_VALIDACAO = "",
            BL_VALIDADO = true,
            CD_STATUS = true,
            DH_TIMESTAMP = DateTime.UtcNow,
            ID_FACEBOOK = facebook.Id_Usuario,
            ID_GOOGLE = "",
            ID_APPLE = ""
        };
        try
        {
            await _usuarioService.CadastraExterno(usuario);
            return Ok(new { code = StatusCode(200), token = token });
        }
        catch (Exception ex)
        {
            return BadRequest(new { code = StatusCode(400), mensage = "Erro ao conectar a conta externa." });
        }
    }

    [HttpPost("EmailRecuperacao/{tokenConcessao}")]
    public async Task<IActionResult> RecuperaSenha([FromRoute] string tokenConcessao, [FromBody] string emailUsuario)
    {
        if (!_usuarioService.VerificaEmail(emailUsuario))
        {
            return BadRequest(new { code = StatusCode(400), mensage = "Usuário não encontrado." });
        }
        try
        {
            var tokenRecebido = _usuarioService.GetTokenValidate(emailUsuario);
            var id = _usuarioService.BuscarIdUsuarioPorEmail(emailUsuario);
            if (tokenConcessao == "1")
            {
                var newBody = _emailService.NewPassWordBody(tokenConcessao, $"https://pedagiopay.com/alterar-senha/{id}");
                await _emailService.NewEmail(emailUsuario, newBody, "Redefinição de Senha");
                //var tokenUser = _repositoryUsuario.GetTokenUsuario;
                return Ok(new { code = StatusCode(200), mensage = "Link enviado para o email de cadastro." });
            }
            if (tokenConcessao == "2")
            {
                var newBody = _emailService.NewPassWordBody(tokenConcessao, $"https://pedagiopay.com/alterar-senha/ecoponte/{id}");
                await _emailService.NewEmail(emailUsuario, newBody, "Redefinição de Senha");
                //var tokenUser = _repositoryUsuario.GetTokenUsuario;
                return Ok(new { code = StatusCode(200), mensage = "Link enviado para o email de cadastro." });
            }
            if (tokenConcessao == "3")
            {
                var newBody = _emailService.NewPassWordBody(tokenConcessao, $"https://pedagiopay.com/alterar-senha/econoroeste/{id}");
                await _emailService.NewEmail(emailUsuario, newBody, "Redefinição de Senha");
                //var tokenUser = _repositoryUsuario.GetTokenUsuario;
                return Ok(new { code = StatusCode(200), mensage = "Link enviado para o email de cadastro." });
            }
            return BadRequest(new { code = StatusCode(400), mensage = "Usuário não encontrado." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { code = StatusCode(401), mensage = "Usuário não encontrado." });
        }
    }
    [HttpPost("TokenResetValidate")]
    public IActionResult TokenResetValidate([FromHeader] string token )
    {
        try
        {
            var tokenRecebido = _usuarioService.GetTokenValidate(token);
            if (token == tokenRecebido)
            {
                return Ok(new { code = StatusCode(200) });
            }
            return BadRequest(new { code = StatusCode(401), mensage = "Não autenticado" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { code = StatusCode(400), mensage = ""});
        }
    }

    [HttpPost("AlteraSenha")]
    public IActionResult AlteraSenha(AlterarSenhaDto request)
    {
        try
        {
            var novaSenha = _usuarioService.AtualizaSenha(request.NovaSenha, request.Id, request.EmailUsuario);
            if (novaSenha == true)
            {
                return Ok(new { code = StatusCode(200), mensage = "Senha atualizada." });
            }
            else
            {
                return BadRequest(new { code = StatusCode(401), mensage = "Não foi possível alterar senha. Usuário inexistente ou incorreto." });
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new { code = StatusCode(400), mensage = "Não foi possível alterar senha." });
        }
    }

    [HttpPost("AtualizaDados")]
    public IActionResult AtualizaDados(UpdateUsuarioDto request)
    {
        try
        {
            _usuarioService.Atualiza(request);
            return Ok(new { code = StatusCode(200), mensage = "Dados atualizados" });
        }catch (Exception ex)
        {
            return BadRequest(new { code = StatusCode(400), mensage = "Não foi possível atualizar dados." });
        }
    }

    [HttpPost("VerificaCadastro")]
    public ActionResult VerificaCadastro([FromBody] string token)
    {
        if (_usuarioService.VerificaCadastro(token))
        {
            return Ok(new { code = StatusCode(200), mensage = "Seu cadastro está completo." });
        }
        return BadRequest(new { code = StatusCode(401), mensage = "Complete o cadastro." });
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

    [HttpGet("minha-conta")]
    public IActionResult MinhaConta([FromHeader] string token)
    {
        try
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token não fornecido.");
            }

            var usuarioDTO = _usuarioService.GetUsuario(token);

            if (usuarioDTO == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            return Ok(new
            {
                nome = usuarioDTO.Nome,
                email = usuarioDTO.Email,
                cpf_cnpj = usuarioDTO.Cpf_Cnpj,
                celular = usuarioDTO.Celular,
                Id = usuarioDTO.Id,
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao processar a solicitação.");
        }
    }
}
