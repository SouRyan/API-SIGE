using API.SIGE.DTOs;
using API.SIGE.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.SIGE.Controllers;

[ApiController]
[Route("api/anexo")]
public class AnexoApiController : ControllerBase
{
    private readonly IAnexoService _anexoService;

    public AnexoApiController(IAnexoService anexoService)
    {
        _anexoService = anexoService;
    }

    [HttpPost("upload")]
    [RequestSizeLimit(20L * 1024 * 1024)]
    public async Task<ActionResult<AnexoResponseDto>> Upload([FromForm] AnexoUploadDto dto)
    {
        try
        {
            var a = await _anexoService.UploadAsync(dto);
            return Ok(a);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("medicao/{medicaoId:int}")]
    public async Task<ActionResult<List<AnexoResponseDto>>> GetByMedicao(int medicaoId)
    {
        return Ok(await _anexoService.GetByMedicaoIdAsync(medicaoId));
    }

    [HttpGet("producao/{producaoId:int}")]
    public async Task<ActionResult<List<AnexoResponseDto>>> GetByProducao(int producaoId)
    {
        return Ok(await _anexoService.GetByProducaoFamiliaIdAsync(producaoId));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _anexoService.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
