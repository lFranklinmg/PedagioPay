using Microsoft.AspNetCore.Mvc;
using PedagioPayPortalUsuario.Data.Dtos;
using PedagioPayPortalUsuario.Data.Dtos.Debitos;
using PedagioPayPortalUsuario.Data.Dtos.Usuario;
using PedagioPayPortalUsuario.Models;
using PedagioPayPortalUsuario.Service.Interfaces;
using System.Numerics;
using System.Runtime.ConstrainedExecution;
using System.Text.RegularExpressions;

namespace PedagioPayPortalUsuario.Controllers;

[ApiController]
[Route("[controller]")]
public class DebitosController : ControllerBase
{
    private PEDAGIOPAYContext _context;
    private readonly IUsuarioService _usuarioService;
    private readonly IDebitoService _debitoService;

    public DebitosController(PEDAGIOPAYContext context, 
        IUsuarioService usuarioService,
        IDebitoService debitoService)
    {
        _context = context;
        _debitoService = debitoService;
        _usuarioService = usuarioService;
    }

    [HttpGet("ConsultaDebitos/{tokenConcessao}")]
    public ActionResult ConsultaDebitos([FromRoute] string tokenConcessao,[FromHeader] string token)
    {
        if (token != null)
        {
            try
            {
              var debitosUsuario =  _debitoService.BuscaDebitos(tokenConcessao, token);
                return Ok(new { code = StatusCode(200), debitos = debitosUsuario.ToList() });
            }catch (Exception ex)
            {
                return BadRequest(new { code = StatusCode(400), mensagem = "Nenhum Debito encontrado." });
            }
        }
        return BadRequest(new { code = StatusCode(401), mensagem = "Nenhum Debito encontrado." });
    }

    [HttpGet("Historico/{tokenConcessao}")]
    public ActionResult HistoricoDebitos([FromRoute] string tokenConcessao, [FromHeader] string token)
    {
        if (token != null)
        {
            try
            {
                var debitosUsuario = _debitoService.HistoricoDebitos(tokenConcessao, token);
                return Ok(new { code = StatusCode(200), debitos = debitosUsuario });
            }
            catch (Exception ex)
            {
                return BadRequest(new { code = StatusCode(400), mensagem = "Nenhum Debito encontrado." });
            }
        }
        return BadRequest(new { code = StatusCode(401), mensagem = "Nenhum Debito encontrado." });
    }

    [HttpPost("ConsultaAvulsa/{tokenConcessao}")]
    public ActionResult ConsultaAvulsa([FromRoute] string tokenConcessao, ConsultaAvulsaDto consulta)
    {
        try
        {
            var debitosUsuario = _debitoService.ConsultaAvulsa(tokenConcessao, consulta);
            return Ok(new { code = StatusCode(200), debitos = debitosUsuario });
        }
        catch (Exception ex) 
        {
            return BadRequest(new { code = StatusCode(401), mensagem = "Nenhum Debito encontrado." });
        }
    }
}
