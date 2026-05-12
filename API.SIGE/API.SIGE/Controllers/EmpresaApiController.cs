using API.SIGE.DTOs;
using API.SIGE.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.SIGE.Controllers;

[Authorize]
[ApiController]
[Route("api/empresa")]
public class EmpresaApiController : ControllerBase
{
    private readonly IEmpresaService _empresaService;

    public EmpresaApiController(IEmpresaService empresaService)
    {
        _empresaService = empresaService;
    }

    [HttpGet]
    public async Task<ActionResult<List<EmpresaResponseDto>>> GetAll()
    {
        return Ok(await _empresaService.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EmpresaResponseDto>> GetById(int id)
    {
        var empresa = await _empresaService.GetByIdAsync(id);
        if (empresa == null) return NotFound();
        return Ok(empresa);
    }

    [HttpPost]
    public async Task<ActionResult<EmpresaResponseDto>> Create([FromBody] EmpresaCreateDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var empresa = await _empresaService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = empresa.IdEmpresa }, empresa);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] EmpresaCreateDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        try
        {
            await _empresaService.UpdateAsync(id, dto);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _empresaService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPatch("{id:int}/inativar")]
    public async Task<ActionResult> Inativar(int id)
    {
        try
        {
            await _empresaService.InativarAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPatch("{id:int}/ativar")]
    public async Task<ActionResult> Ativar(int id)
    {
        try
        {
            await _empresaService.AtivarAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
