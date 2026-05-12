using API.SIGE.DTOs;
using API.SIGE.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.SIGE.Controllers;

[ApiController]
[Route("api/solicitacao-cadastro")]
public class SolicitacaoCadastroApiController : ControllerBase
{
    private readonly ISolicitacaoCadastroService _service;

    public SolicitacaoCadastroApiController(ISolicitacaoCadastroService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<SolicitacaoCadastroResponseDto>> Solicitar([FromBody] SolicitacaoCadastroCreateDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        try
        {
            var resultado = await _service.SolicitarAsync(dto);
            return Ok(resultado);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("status/{cnpj}")]
    public async Task<ActionResult> ConsultarStatus(string cnpj)
    {
        var todas = await _service.GetAllAsync();
        var solicitacao = todas
            .Where(s => s.Cnpj == cnpj)
            .OrderByDescending(s => s.DataSolicitacao)
            .FirstOrDefault();

        if (solicitacao == null)
            return NotFound(new { message = "Nenhuma solicitação encontrada para este CNPJ." });

        return Ok(new
        {
            solicitacao.Status,
            solicitacao.DataSolicitacao,
            solicitacao.DataAnalise,
            solicitacao.MotivoRecusa
        });
    }
}
