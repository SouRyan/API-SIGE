using API.SIGE.Interfaces;
using API.SIGE.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace SIGE.API.ApiControllers;

[EnableCors("MyPolicy")]
[ApiController]
[Route("api/[controller]")]
public class CaixilhoApiController : ControllerBase
{
    private readonly ICaixilhoRepository _caixilhoRepository;
    private readonly IObraRepository _obraRepository;

    public CaixilhoApiController(ICaixilhoRepository caixilhoRepository, IObraRepository obraRepository)
    {
        _caixilhoRepository = caixilhoRepository;
        _obraRepository = obraRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<Caixilho>>> GetAll()
    {
        var lista = await _caixilhoRepository.GetAllAsync();
        return Ok(lista);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Caixilho>> GetById(int id)
    {
        var item = await _caixilhoRepository.GetById(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] Caixilho caixilho)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var obra = await _obraRepository.GetById(caixilho.ObraId);
        if (obra == null) return BadRequest("ObraId inválido.");

        await _caixilhoRepository.AddAsync(caixilho);
        return CreatedAtAction(nameof(GetById), new { id = caixilho.IdCaixilho }, caixilho);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] Caixilho caixilho)
    {
        if (id != caixilho.IdCaixilho) return BadRequest("ID inválido.");
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var existente = await _caixilhoRepository.GetById(id);
        if (existente == null) return NotFound();

        await _caixilhoRepository.UpdateAsync(caixilho);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var existente = await _caixilhoRepository.GetById(id);
        if (existente == null) return NotFound();

        await _caixilhoRepository.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("{id:int}/liberar")]
    public async Task<ActionResult> Liberar(int id)
    {
        var caixilho = await _caixilhoRepository.GetById(id);
        if (caixilho == null) return NotFound();

        caixilho.Liberado = true;
        caixilho.DataLiberacao = DateTime.Now;
        //caixilho.StatusProducao = "Liberado";
        await _caixilhoRepository.UpdateAsync(caixilho);

        return Ok(new { success = true });
    }
}
