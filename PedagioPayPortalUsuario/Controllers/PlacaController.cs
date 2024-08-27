using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PedagioPayPortalUsuario.Data.Dtos;
using PedagioPayPortalUsuario.Models;
using PedagioPayPortalUsuario.Service.Interfaces;

namespace PedagioPayPortalUsuario.Controllers;

[ApiController]
[Route("[controller]")]
public class PlacaController : ControllerBase
{

    private PEDAGIOPAYContext _context;
    private readonly IPlacaService _placaService;
    private readonly IUsuarioService _usuarioService;

    public PlacaController(PEDAGIOPAYContext context,
        IPlacaService placaService,
        IUsuarioService usuarioService)
    {
        _context = context;
        _placaService = placaService;
        _usuarioService = usuarioService;
    }

    [HttpPost("Cadastro")]
    public async Task<ActionResult> Cadastro(PlacaDto placaDto)
    {
        try
        {
            PLACAs placa = new PLACAs()
            {
                TOKEN_USUARIO = placaDto.TokenUsuario,
                PLACA = placaDto.Placa,
                MARCA = placaDto.Marca,
                MODELO = placaDto.Modelo,
                ANO = placaDto.Ano,
                COR = placaDto.Cor
            };
            var novaPlaca = _placaService.Cadastra(placa);
            if (novaPlaca == null)
            {
                return Ok(new { code = StatusCode(200), mensage = "Placa não preenchida corretamente ou já vinculada ao usuário." });
            }
            return Ok(new { code = StatusCode(200), mensage = "Placa adicionada com sucesso." });
         
        }
        catch (Exception ex)
        {
            return BadRequest(new { code = StatusCode(401), mensage = "Erro ao adicionar placa. Tente novamente." });
        }
    }

    [HttpGet("BuscarPlacas")]
     public ActionResult BuscarPlacas([FromHeader] string token)
     {
        try
        {
            var placas = _placaService.BuscarPlaca(token);
            if ( placas != null)
            {
                return Ok(new { code = StatusCode(200), placa = placas});
            }
            return Ok(new { code = StatusCode(200), mensage = "Nenhuma placa cadastrada." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { code = StatusCode(400), mensage = "Erro ao encontrar placas." });
        }
    }

    [HttpPost("ConsultaAvulsa")]
    public ActionResult Consulta([FromBody] string placa)
    {
        var valor = "";

        return Ok(new { code = StatusCode(200), mensage = "", val = valor });
    }

    [HttpPost("RemoverPlaca")]
    public ActionResult RemoverPlaca(int idPlaca)
    {
        try
        {
            if (idPlaca == null)
            {
                return BadRequest(new { code = StatusCode(401), mensage = "Informe uma placa" });
            }
            _placaService.ExcluirPlaca(idPlaca);
            return Ok(new { code = StatusCode(200), mensage = "Placa removida com sucesso." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { code = StatusCode(401), mensage = "Não foi possível remover esta placa. Tente novamente mais tarde." });
        }
    }

    [HttpPost("AtualizaPlaca")]
    public ActionResult AtualizaPlaca(UpdatePlacaDto request)
    {
        if(_placaService.AtualizaPlaca(request))
        {
            return Ok(new { code = StatusCode(200), mensage = "Placa atualizada com sucesso." });
        }
        else
        {
            return BadRequest(new {mensage = "Usuário não encontrado" });
        }
    }
}
