using API.SIGE.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using API.SIGE.DTOs;
namespace API.SIGE.Controllers;

[Authorize]
[ApiController]
[Route("api/solicitacao")]
public class SolicitacaoClienteApiController : ControllerBase
{
    private readonly ISolicitacaoClienteService _service;

    public SolicitacaoClienteApiController(ISolicitacaoClienteService service)
    {
        _service = service;
    }

    private int GetClienteId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        return int.Parse(claim!.Value);
    }

    // GET /api/solicitacao/minhas
    [HttpGet("minhas")]
    public async Task<ActionResult<List<SolicitacaoClienteResponseDto>>> GetMinhas()
    {
        var clienteId = GetClienteId();
        var lista = await _service.GetByClienteIdAsync(clienteId);
        return Ok(lista);
    }

    // GET /api/solicitacao/familia/{familiaId}
    [HttpGet("familia/{familiaId:int}")]
    public async Task<ActionResult<List<SolicitacaoClienteResponseDto>>> GetByFamilia(int familiaId)
    {
        var lista = await _service.GetByFamiliaIdAsync(familiaId);
        return Ok(lista);
    }

    // POST /api/solicitacao
    [HttpPost]
    public async Task<ActionResult<SolicitacaoClienteResponseDto>> Create([FromBody] SolicitacaoClienteCreateDto dto)
    {
        try
        {
            var clienteId = GetClienteId();
            var resultado = await _service.CreateAsync(clienteId, dto);
            return CreatedAtAction(nameof(GetMinhas), resultado);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PUT /api/solicitacao/{id}
    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] SolicitacaoClienteCreateDto dto)
    {
        try
        {
            var clienteId = GetClienteId();
            await _service.UpdateAsync(id, clienteId, dto);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // DELETE /api/solicitacao/{id}
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var clienteId = GetClienteId();
            await _service.DeleteAsync(id, clienteId);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}