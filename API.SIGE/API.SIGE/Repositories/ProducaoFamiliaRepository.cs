using API.SIGE.Data;
using API.SIGE.Interfaces.Repositories;
using API.SIGE.Models;
using Microsoft.EntityFrameworkCore;

namespace API.SIGE.Repositories;

public class ProducaoFamiliaRepository : IProducaoFamiliaRepository
{
    private readonly AppDbData _context;

    public ProducaoFamiliaRepository(AppDbData context)
    {
        _context = context;
    }

    public async Task<List<ProducaoFamilia>> GetAllAsync() =>
        await _context.ProducoesFamilia
            .Include(p => p.FamiliaCaixilho)
            .Include(p => p.Responsavel)
            .ToListAsync();

    public async Task<ProducaoFamilia?> GetByIdAsync(int id) =>
        await _context.ProducoesFamilia
            .Include(p => p.FamiliaCaixilho)
            .Include(p => p.Responsavel)
            .FirstOrDefaultAsync(p => p.IdProducaoFamilia == id);

    public async Task<ProducaoFamilia?> GetByFamiliaIdAsync(int familiaId) =>
        await _context.ProducoesFamilia
            .Include(p => p.FamiliaCaixilho)
            .Include(p => p.Responsavel)
            .OrderByDescending(p => p.IdProducaoFamilia)
            .FirstOrDefaultAsync(p => p.IdFamiliaCaixilho == familiaId);

    public async Task AddAsync(ProducaoFamilia producao)
    {
        await _context.ProducoesFamilia.AddAsync(producao);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ProducaoFamilia producao)
    {
        _context.ProducoesFamilia.Update(producao);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.ProducoesFamilia.FindAsync(id);
        if (entity != null)
        {
            _context.ProducoesFamilia.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
