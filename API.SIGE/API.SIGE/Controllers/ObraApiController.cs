using GerenciamentoProducao.Services;
using API.SIGE.Interfaces;
using SIGE.API.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace API.SIGE.Controllers;

[EnableCors("MyPolicy")]
[ApiController]
[Route("api/[controller]")]
public class ObraApiController : ControllerBase
{
    private readonly IObraRepository _obraRepository;
    private readonly GoogleCalendarService _calendarService;
    private readonly string _calendarId;

    public ObraApiController(
        IObraRepository obraRepository,
        GoogleCalendarService calendarService,
        IConfiguration configuration)
    {
        _obraRepository = obraRepository;
        _calendarService = calendarService;
        _calendarId = configuration["Google:key"] ?? string.Empty;
    }

    [HttpGet]
    public async Task<ActionResult<List<Obra>>> GetAll([FromQuery] bool? finalizadas = null)
    {
        var obras = finalizadas switch
        {
            true => await _obraRepository.GetAllFinalizadosAsync(),
            false => await _obraRepository.GetAllNaoFinalizadosAsync(),
            _ => await _obraRepository.GetAllAsync()
        };

        return Ok(obras.OrderByDescending(o => o.IdObra));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Obra>> GetById(int id)
    {
        var obra = await _obraRepository.GetById(id);
        if (obra == null) return NotFound();
        return Ok(obra);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] Obra obra)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        if (!string.IsNullOrWhiteSpace(_calendarId))
        {
            try
            {
                var ev = _calendarService.CreateEvent(
                    _calendarId,
                    $"Obra: {obra.Nome}",
                    obra.DataInicio,
                    obra.DataTermino,
                    $"Construtora: {obra.Construtora}");

                obra.GoogleCalendarEventId = ev.Id;
            }
            catch
            {
                // Falha no calendar nao bloqueia a criacao da obra.
            }
        }

        await _obraRepository.AddAsync(obra);
        return CreatedAtAction(nameof(GetById), new { id = obra.IdObra }, obra);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] Obra obra)
    {
        if (id != obra.IdObra) return BadRequest("ID inválido.");
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var existente = await _obraRepository.GetById(id);
        if (existente == null) return NotFound();

        await _obraRepository.UpdateAsync(obra);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var obra = await _obraRepository.GetById(id);
        if (obra == null) return NotFound();

        if (!string.IsNullOrWhiteSpace(_calendarId) && !string.IsNullOrWhiteSpace(obra.GoogleCalendarEventId))
        {
            try
            {
                _calendarService.DeleteEvent(_calendarId, obra.GoogleCalendarEventId);
            }
            catch
            {
                // Falha no calendar nao bloqueia a exclusao da obra.
            }
        }

        await _obraRepository.DeleteAsync(id);
        return NoContent();
    }
}
