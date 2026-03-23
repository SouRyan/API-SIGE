
using API.SIGE.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SIGE.API.Models;

namespace API.SIGE.Controllers;

[EnableCors("MyPolicy")]
[ApiController]
[Route("api/[controller]")]
public class TipoCaixilhoApiController : ControllerBase
{
    private readonly ITipoCaixilhoRepository _tipoCaixilhoRepository;

    public TipoCaixilhoApiController(ITipoCaixilhoRepository tipoCaixilhoRepository)
    {
        _tipoCaixilhoRepository = tipoCaixilhoRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<TipoCaixilho>>> GetAll()
    {
        var lista = await _tipoCaixilhoRepository.GetAllAsync();
        return Ok(lista);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TipoCaixilho>> GetById(int id)
    {
        var item = await _tipoCaixilhoRepository.GetById(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] TipoCaixilho tipoCaixilho)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        await _tipoCaixilhoRepository.AddAsync(tipoCaixilho);
        return CreatedAtAction(nameof(GetById), new { id = tipoCaixilho.IdTipoCaixilho }, tipoCaixilho);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] TipoCaixilho tipoCaixilho)
    {
        if (id != tipoCaixilho.IdTipoCaixilho) return BadRequest("ID inválido.");
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var existente = await _tipoCaixilhoRepository.GetById(id);
        if (existente == null) return NotFound();

        await _tipoCaixilhoRepository.UpdateAsync(tipoCaixilho);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var existente = await _tipoCaixilhoRepository.GetById(id);
        if (existente == null) return NotFound();

        await _tipoCaixilhoRepository.DeleteAsync(id);
        return NoContent();
    }
}
