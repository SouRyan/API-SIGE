using API.SIGE.Data;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.SIGE.Controllers;

[EnableCors("MyPolicy")]
[ApiController]
[Route("api/[controller]")]
public class DashboardApiController : ControllerBase
{
    private readonly AppDbData _context;

    public DashboardApiController(AppDbData context)
    {
        _context = context;
    }

    [HttpGet("metricas")]
    public async Task<ActionResult> GetMetricas()
    {
        var totalObras = await _context.Obras.CountAsync();
        var totalProducoes = await _context.Producoes.CountAsync();
        var totalCaixilhos = await _context.Caixilhos.CountAsync();
        var totalUsuarios = await _context.Usuarios.CountAsync(u => u.Ativo);

        return Ok(new
        {
            totalObras,
            totalProducoes,
            totalCaixilhos,
            totalUsuarios
        });
    }

    [HttpGet("producao-por-mes")]
    public async Task<ActionResult> GetProducaoPorMes()
    {
        var data = await _context.Producoes
            .GroupBy(p => new { p.DataProducao.Year, p.DataProducao.Month })
            .Select(g => new
            {
                ano = g.Key.Year,
                mes = g.Key.Month,
                total = g.Count(),
                concluidas = g.Count(x => x.Produzido)
            })
            .OrderBy(x => x.ano)
            .ThenBy(x => x.mes)
            .Take(12)
            .ToListAsync();

        return Ok(data);
    }

    [HttpPost("atualizar-progresso-obras")]
    public async Task<ActionResult> AtualizarProgressoObras()
    {
        var obras = await _context.Obras.ToListAsync();
        foreach (var obra in obras)
        {
            var pesoProduzido = await _context.Caixilhos
                .Where(c => c.ObraId == obra.IdObra && c.Liberado)
                .SumAsync(c => c.PesoUnitario * c.Quantidade);

            obra.PesoProduzido = pesoProduzido;
            obra.PercentualConclusao = obra.PesoFinal > 0
                ? Math.Min(100, (pesoProduzido / obra.PesoFinal) * 100)
                : 0;
        }

        await _context.SaveChangesAsync();
        return Ok(new { success = true });
    }
}
