using API.SIGE.DTOs;
using API.SIGE.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.SIGE.Controllers;

[Authorize]
[ApiController]
[Route("api/admin")]
public class AdminApiController : ControllerBase
{
    private readonly ISolicitacaoCadastroService _solicitacaoService;
    private readonly IAdminService _adminService;

    public AdminApiController(
        ISolicitacaoCadastroService solicitacaoService,
        IAdminService adminService)
    {
        _solicitacaoService = solicitacaoService;
        _adminService = adminService;
    }

    [HttpGet("solicitacoes")]
    public async Task<ActionResult<List<SolicitacaoCadastroResponseDto>>> GetSolicitacoes()
    {
        var lista = await _solicitacaoService.GetAllAsync();
        return Ok(lista);
    }

    [HttpGet("solicitacoes/pendentes")]
    public async Task<ActionResult<List<SolicitacaoCadastroResponseDto>>> GetPendentes()
    {
        var lista = await _solicitacaoService.GetPendentesAsync();
        return Ok(lista);
    }

    [HttpGet("solicitacoes/{id:int}")]
    public async Task<ActionResult<SolicitacaoCadastroResponseDto>> GetSolicitacaoById(int id)
    {
        var s = await _solicitacaoService.GetByIdAsync(id);
        if (s == null) return NotFound();
        return Ok(s);
    }

    [HttpPost("solicitacoes/{id:int}/aprovar")]
    public async Task<ActionResult<EmpresaResponseDto>> Aprovar(int id, [FromBody] AprovarSolicitacaoDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        try
        {
            var empresa = await _adminService.AprovarSolicitacaoAsync(id, dto);
            return Ok(empresa);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("solicitacoes/{id:int}/recusar")]
    public async Task<ActionResult> Recusar(int id, [FromBody] RecusarSolicitacaoDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        try
        {
            await _solicitacaoService.RecusarAsync(id, dto.MotivoRecusa);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
