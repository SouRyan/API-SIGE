using API.SIGE.DTOs;
using API.SIGE.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.SIGE.Controllers;

[Authorize]
[ApiController]
[Route("api/producao-familia")]
public class ProducaoFamiliaApiController : ControllerBase
{
    private readonly IProducaoFamiliaService _producaoFamiliaService;

    public ProducaoFamiliaApiController(IProducaoFamiliaService producaoFamiliaService)
    {
        _producaoFamiliaService = producaoFamiliaService;
    }

    [HttpGet("familia/{familiaId:int}")]
    public async Task<ActionResult<ProducaoFamiliaResponseDto>> GetByFamilia(int familiaId)
    {
        var p = await _producaoFamiliaService.GetByFamiliaIdAsync(familiaId);
        if (p == null) return NotFound();
        return Ok(p);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProducaoFamiliaResponseDto>> GetById(int id)
    {
        var p = await _producaoFamiliaService.GetByIdAsync(id);
        if (p == null) return NotFound();
        return Ok(p);
    }

    [HttpPost("{familiaId:int}/iniciar")]
    public async Task<ActionResult<ProducaoFamiliaResponseDto>> Iniciar(int familiaId, [FromBody] ProducaoFamiliaIniciarDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        try
        {
            var p = await _producaoFamiliaService.IniciarAsync(familiaId, dto);
            return Ok(p);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{familiaId:int}/pausar")]
    public async Task<ActionResult<ProducaoFamiliaResponseDto>> Pausar(int familiaId, [FromBody] ProducaoFamiliaPausarDto? dto)
    {
        try
        {
            var p = await _producaoFamiliaService.PausarAsync(familiaId, dto);
            return Ok(p);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{familiaId:int}/finalizar")]
    public async Task<ActionResult<ProducaoFamiliaResponseDto>> Finalizar(int familiaId, [FromBody] ProducaoFamiliaFinalizarDto? dto)
    {
        dto ??= new ProducaoFamiliaFinalizarDto();
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        try
        {
            var p = await _producaoFamiliaService.FinalizarAsync(familiaId, dto);
            return Ok(p);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
