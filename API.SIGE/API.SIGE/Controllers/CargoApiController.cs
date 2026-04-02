using API.SIGE.DTOs;
using API.SIGE.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.SIGE.Controllers;

[Authorize]
[ApiController]
[Route("api/cargo")]
public class CargoApiController : ControllerBase
{
    private readonly ICargoService _cargoService;

    public CargoApiController(ICargoService cargoService)
    {
        _cargoService = cargoService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CargoResponseDto>>> GetAll()
    {
        return Ok(await _cargoService.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CargoResponseDto>> GetById(int id)
    {
        var c = await _cargoService.GetByIdAsync(id);
        if (c == null) return NotFound();
        return Ok(c);
    }

    [HttpPost]
    public async Task<ActionResult<CargoResponseDto>> Create([FromBody] CargoCreateDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var c = await _cargoService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = c.IdCargo }, c);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] CargoCreateDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        await _cargoService.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _cargoService.DeleteAsync(id);
        return NoContent();
    }
}
