using API.SIGE.DTOs;
using API.SIGE.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.SIGE.Controllers;

[ApiController]
[Route("api/medicao")]
public class MedicaoApiController : ControllerBase
{
    private readonly IMedicaoService _medicaoService;

    public MedicaoApiController(IMedicaoService medicaoService)
    {
        _medicaoService = medicaoService;
    }

    [HttpGet("familia/{familiaId:int}")]
    public async Task<ActionResult<MedicaoResponseDto>> GetByFamilia(int familiaId)
    {
        var m = await _medicaoService.GetByFamiliaIdAsync(familiaId);
        if (m == null) return NotFound();
        return Ok(m);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MedicaoResponseDto>> GetById(int id)
    {
        var m = await _medicaoService.GetByIdAsync(id);
        if (m == null) return NotFound();
        return Ok(m);
    }

    [HttpPost("{familiaId:int}/iniciar")]
    public async Task<ActionResult<MedicaoResponseDto>> Iniciar(int familiaId, [FromBody] MedicaoIniciarDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        try
        {
            var m = await _medicaoService.IniciarAsync(familiaId, dto);
            return Ok(m);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{familiaId:int}/pausar")]
    public async Task<ActionResult<MedicaoResponseDto>> Pausar(int familiaId, [FromBody] MedicaoPausarDto? dto)
    {
        try
        {
            var m = await _medicaoService.PausarAsync(familiaId, dto);
            return Ok(m);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{familiaId:int}/finalizar")]
    public async Task<ActionResult<MedicaoResponseDto>> Finalizar(int familiaId, [FromBody] MedicaoFinalizarDto? dto)
    {
        dto ??= new MedicaoFinalizarDto();
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        try
        {
            var m = await _medicaoService.FinalizarAsync(familiaId, dto);
            return Ok(m);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
