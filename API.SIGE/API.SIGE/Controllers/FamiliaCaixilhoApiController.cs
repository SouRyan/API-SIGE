using API.SIGE.DTOs;
using API.SIGE.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.SIGE.Controllers;

[Authorize]
[ApiController]
[Route("api/familia-caixilho")]
public class FamiliaCaixilhoApiController : ControllerBase
{
    private readonly IFamiliaCaixilhoService _familiaService;

    public FamiliaCaixilhoApiController(IFamiliaCaixilhoService familiaService)
    {
        _familiaService = familiaService;
    }

    [HttpGet]
    public async Task<ActionResult<List<FamiliaCaixilhoResponseDto>>> GetAll()
    {
        var lista = await _familiaService.GetAllAsync();
        return Ok(lista);
    }

    [HttpGet("obra/{obraId:int}")]
    public async Task<ActionResult<List<FamiliaCaixilhoResponseDto>>> GetByObra(int obraId)
    {
        var lista = await _familiaService.GetByObraIdAsync(obraId);
        return Ok(lista);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<FamiliaCaixilhoResponseDto>> GetById(int id)
    {
        var item = await _familiaService.GetByIdAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<FamiliaCaixilhoResponseDto>> Create([FromBody] FamiliaCaixilhoCreateDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var familia = await _familiaService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = familia.IdFamiliaCaixilho }, familia);
    }

    [HttpPost("{id:int}/liberar")]
    public Task<ActionResult> LiberarParaProducaoCurto(int id) => LiberarParaProducao(id);

    [HttpPost("{id:int}/liberar-para-producao")]
    public async Task<ActionResult> LiberarParaProducao(int id)
    {
        try
        {
            await _familiaService.LiberarParaProducaoAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{id:int}/finalizar")]
    public Task<ActionResult> FinalizarProducaoCurto(int id) => FinalizarProducao(id);

    [HttpPost("{id:int}/finalizar-producao")]
    public async Task<ActionResult> FinalizarProducao(int id)
    {
        try
        {
            await _familiaService.FinalizarProducaoAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] FamiliaCaixilhoUpdateDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var existente = await _familiaService.GetByIdAsync(id);
        if (existente == null) return NotFound();

        await _familiaService.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var existente = await _familiaService.GetByIdAsync(id);
        if (existente == null) return NotFound();

        await _familiaService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("recalcular-pesos")]
    public async Task<ActionResult> RecalcularPesos()
    {
        var total = await _familiaService.RecalcularPesosAsync();
        return Ok(new { success = true, total });
    }
}
