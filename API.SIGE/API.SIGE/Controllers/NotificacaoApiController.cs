using API.SIGE.DTOs;
using API.SIGE.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.SIGE.Controllers;

[Authorize]
[ApiController]
[Route("api/notificacao")]
public class NotificacaoApiController : ControllerBase
{
    private readonly INotificacaoService _notificacaoService;

    public NotificacaoApiController(INotificacaoService notificacaoService)
    {
        _notificacaoService = notificacaoService;
    }

    [HttpGet("{idUsuario:int}")]
    public async Task<ActionResult> GetByUsuario(int idUsuario)
    {
        var lista = await _notificacaoService.GetByUsuarioIdAsync(idUsuario);
        return Ok(lista);
    }

    [HttpGet("{idUsuario:int}/nao-lidas/count")]
    public async Task<ActionResult> GetNaoLidasCount(int idUsuario)
    {
        var count = await _notificacaoService.GetNaoLidasCountAsync(idUsuario);
        return Ok(new { count });
    }

    [HttpPost("{id:int}/marcar-lida")]
    public async Task<ActionResult> MarcarLida(int id)
    {
        try
        {
            await _notificacaoService.MarcarLidaAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("broadcast")]
    public async Task<ActionResult> Broadcast([FromBody] NotificacaoBroadcastDto dto)
    {
        await _notificacaoService.BroadcastAsync(
            dto.Titulo, dto.Mensagem, dto.TipoNotificacao, dto.IdObra, dto.TipoCargo);
        return NoContent();
    }
}
