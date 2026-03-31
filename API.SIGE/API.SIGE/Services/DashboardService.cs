using API.SIGE.Data;
using API.SIGE.DTOs;
using API.SIGE.Interfaces.Services;
using API.SIGE.Model;
using Microsoft.EntityFrameworkCore;

namespace API.SIGE.Services;

public class DashboardService : IDashboardService
{
    private readonly AppDbData _context;
    private readonly IObraService _obraService;

    public DashboardService(AppDbData context, IObraService obraService)
    {
        _context = context;
        _obraService = obraService;
    }

    public async Task<DashboardMetricasDto> GetMetricasAsync()
    {
        var totalObras = await _context.Obras.CountAsync();
        var totalCaixilhos = await _context.Caixilhos.CountAsync();
        var totalUsuarios = await _context.Usuarios.CountAsync();
        var totalProducoes = await _context.ProducoesFamilia.CountAsync();

        var obras = await _context.Obras.AsNoTracking().ToListAsync();
        var obrasEmAndamento = obras.Count(o =>
            !o.Finalizado && o.StatusObra != StatusObra.Concluida);

        var obrasProgresso = obras
            .Select(o => new ObraDashboardDto
            {
                IdObra = o.IdObra,
                Nome = o.Nome,
                StatusObra = o.StatusObra,
                PercentualMedicao = o.PercentualMedicao,
                PercentualProducao = o.PercentualProducao
            })
            .OrderByDescending(o => o.PercentualMedicao + o.PercentualProducao)
            .ToList();

        return new DashboardMetricasDto
        {
            TotalObras = totalObras,
            TotalProducoes = totalProducoes,
            TotalCaixilhos = totalCaixilhos,
            TotalUsuarios = totalUsuarios,
            ObrasEmAndamento = obrasEmAndamento,
            ObrasProgresso = obrasProgresso
        };
    }

    public async Task AtualizarProgressoObrasAsync()
    {
        var ids = await _context.Obras.Select(o => o.IdObra).ToListAsync();
        foreach (var id in ids)
            await _obraService.RecalcularProgressoAsync(id);
    }
}
