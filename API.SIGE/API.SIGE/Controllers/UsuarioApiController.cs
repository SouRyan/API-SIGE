using API.SIGE.DTOs;
using API.SIGE.Interfaces.Services;
using API.SIGE.Model;
using Microsoft.AspNetCore.Mvc;

namespace API.SIGE.Controllers;

[ApiController]
[Route("api/usuario")]
public class UsuarioApiController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuarioApiController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet]
    public async Task<ActionResult<List<UsuarioResponseDto>>> GetAll([FromQuery] bool ativos = true)
    {
        var usuarios = ativos
            ? await _usuarioService.GetAllAtivosAsync()
            : await _usuarioService.GetAllAsync();
        return Ok(usuarios);
    }

    [HttpGet("inativos")]
    public async Task<ActionResult<List<UsuarioResponseDto>>> GetInativos()
    {
        var usuarios = await _usuarioService.GetAllInativosAsync();
        return Ok(usuarios);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UsuarioResponseDto>> GetById(int id)
    {
        var usuario = await _usuarioService.GetByIdAsync(id);
        if (usuario == null) return NotFound();
        return Ok(usuario);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto request)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var result = await _usuarioService.LoginAsync(request);
        if (!result.Success)
            return Unauthorized(result);

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<UsuarioResponseDto>> Create([FromBody] UsuarioCreateDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var usuario = await _usuarioService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = usuario.IdUsuario }, usuario);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] UsuarioUpdateDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var existente = await _usuarioService.GetByIdAsync(id);
        if (existente == null) return NotFound();

        await _usuarioService.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Inativar(int id)
    {
        var existente = await _usuarioService.GetByIdAsync(id);
        if (existente == null) return NotFound();

        await _usuarioService.InativarAsync(id);
        return NoContent();
    }

    [HttpPost("{id:int}/ativar")]
    public async Task<ActionResult> Ativar(int id)
    {
        var existente = await _usuarioService.GetByIdAsync(id);
        if (existente == null) return NotFound();

        await _usuarioService.AtivarAsync(id);
        return Ok(new { success = true });
    }

    [HttpPost("{id:int}/cargo/{idCargo:int}")]
    public async Task<ActionResult> AtribuirCargo(int id, int idCargo)
    {
        try
        {
            await _usuarioService.AtribuirCargoAsync(id, idCargo);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}/cargo")]
    public async Task<ActionResult> RemoverCargo(int id)
    {
        try
        {
            await _usuarioService.RemoverCargoAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("cargo/{tipoCargo}")]
    public async Task<ActionResult<List<UsuarioResponseDto>>> GetByCargo(TipoCargo tipoCargo)
    {
        var lista = await _usuarioService.GetByCargoAsync(tipoCargo);
        return Ok(lista);
    }
}
