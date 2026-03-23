using API.SIGE.Interfaces;
using SIGE.API.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace API.SIGE.Controllers;

[EnableCors("MyPolicy")]
[ApiController]
[Route("api/[controller]")]
public class FamiliaCaixilhoApiController : ControllerBase
{
    private readonly IFamiliaCaixilhoRepository _familiaCaixilhoRepository;

    public FamiliaCaixilhoApiController(IFamiliaCaixilhoRepository familiaCaixilhoRepository)
    {
        _familiaCaixilhoRepository = familiaCaixilhoRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<FamiliaCaixilho>>> GetAll()
    {
        var lista = await _familiaCaixilhoRepository.GetAllAsync();
        return Ok(lista);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<FamiliaCaixilho>> GetById(int id)
    {
        var item = await _familiaCaixilhoRepository.GetByIdAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] FamiliaCaixilho familia)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        familia.PesoTotal = 0;
        await _familiaCaixilhoRepository.AddAsync(familia);
        return CreatedAtAction(nameof(GetById), new { id = familia.IdFamiliaCaixilho }, familia);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] FamiliaCaixilho familia)
    {
        if (id != familia.IdFamiliaCaixilho) return BadRequest("ID inválido.");
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var existente = await _familiaCaixilhoRepository.GetByIdAsync(id);
        if (existente == null) return NotFound();

        await _familiaCaixilhoRepository.UpdateAsync(familia);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var existente = await _familiaCaixilhoRepository.GetByIdAsync(id);
        if (existente == null) return NotFound();

        await _familiaCaixilhoRepository.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("recalcular-pesos")]
    public async Task<ActionResult> RecalcularPesos()
    {
        var familias = await _familiaCaixilhoRepository.GetAllAsync();
        foreach (var familia in familias)
        {
            await _familiaCaixilhoRepository.AtualizarPesoTotalAsync(familia.IdFamiliaCaixilho);
        }

        return Ok(new { success = true, total = familias.Count });
    }
}
