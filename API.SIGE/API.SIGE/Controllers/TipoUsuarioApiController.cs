using API.SIGE.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SIGE.API.Models;

namespace API.SIGE.Controllers;

[EnableCors("MyPolicy")]
[ApiController]
[Route("api/[controller]")]
public class TipoUsuarioApiController : ControllerBase
{
    private readonly ITipoUsuarioRepository _tipoUsuarioRepository;

    public TipoUsuarioApiController(ITipoUsuarioRepository tipoUsuarioRepository)
    {
        _tipoUsuarioRepository = tipoUsuarioRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<TipoUsuario>>> GetAll()
    {
        var lista = await _tipoUsuarioRepository.GetAllAsync();
        return Ok(lista);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TipoUsuario>> GetById(int id)
    {
        var item = await _tipoUsuarioRepository.GetById(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] TipoUsuario tipoUsuario)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        await _tipoUsuarioRepository.AddAsync(tipoUsuario);
        return CreatedAtAction(nameof(GetById), new { id = tipoUsuario.IdTipoUsuario }, tipoUsuario);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] TipoUsuario tipoUsuario)
    {
        if (id != tipoUsuario.IdTipoUsuario) return BadRequest("ID inválido.");
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var existente = await _tipoUsuarioRepository.GetById(id);
        if (existente == null) return NotFound();

        await _tipoUsuarioRepository.UpdateAsync(tipoUsuario);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var existente = await _tipoUsuarioRepository.GetById(id);
        if (existente == null) return NotFound();

        await _tipoUsuarioRepository.DeleteAsync(id);
        return NoContent();
    }
}
