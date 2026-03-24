using API.SIGE.Interfaces;
using SIGE.API.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace API.SIGE.Controllers;

[EnableCors("MyPolicy")]
[ApiController]
[Route("api/[controller]")]
public class UsuarioApiController : ControllerBase
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioApiController(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<Usuario>>> GetAll([FromQuery] bool ativos = true)
    {
        var usuarios = ativos
            ? await _usuarioRepository.GetAllAtivosAsync()
            : await _usuarioRepository.GetAllAsync();
        return Ok(usuarios);
    }

    [HttpGet("inativos")]
    public async Task<ActionResult<List<Usuario>>> GetInativos()
    {
        var usuarios = await _usuarioRepository.GetAllInativosAsync();
        return Ok(usuarios);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Usuario>> GetById(int id)
    {
        var usuario = await _usuarioRepository.GetById(id);
        if (usuario == null) return NotFound();
        return Ok(usuario);
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginRequest request)
    {
        var usuario = await _usuarioRepository.ValidarLoginAsync(request.Email, request.Senha);
        if (usuario == null || !usuario.Ativo)
        {
            return Unauthorized(new { success = false, message = "Usuário ou senha inválidos." });
        }

        return Ok(new
        {
            success = true,
            usuario.IdUsuario,
            usuario.NomeUsuario,
            usuario.Email,
            TipoUsuario = usuario.TipoUsuario?.IdTipoUsuario
        });
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] Usuario usuario)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        usuario.Ativo = true;
        await _usuarioRepository.AddAsync(usuario);
        return CreatedAtAction(nameof(GetById), new { id = usuario.IdUsuario }, usuario);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] Usuario usuario)
    {
        if (id != usuario.IdUsuario) return BadRequest("ID inválido.");
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var existente = await _usuarioRepository.GetById(id);
        if (existente == null) return NotFound();

        await _usuarioRepository.UpdateAsync(usuario);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Inativar(int id)
    {
        var existente = await _usuarioRepository.GetById(id);
        if (existente == null) return NotFound();

        await _usuarioRepository.InativarAsync(id);
        return NoContent();
    }

    [HttpPost("{id:int}/ativar")]
    public async Task<ActionResult> Ativar(int id)
    {
        var usuario = await _usuarioRepository.GetById(id);
        if (usuario == null) return NotFound();

        usuario.Ativo = true;
        await _usuarioRepository.UpdateAsync(usuario);
        return Ok(new { success = true });
    }

    public sealed class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
    }
}
