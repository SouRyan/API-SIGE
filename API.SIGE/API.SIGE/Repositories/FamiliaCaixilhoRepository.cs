using API.SIGE.Data;
using API.SIGE.Interfaces.Repositories;
using API.SIGE.Model;
using Microsoft.EntityFrameworkCore;

namespace API.SIGE.Repositories;

public class FamiliaCaixilhoRepository : IFamiliaCaixilhoRepository
{
    private readonly AppDbData _context;

    public FamiliaCaixilhoRepository(AppDbData context)
    {
        _context = context;
    }

    public async Task AddAsync(FamiliaCaixilho familiaCaixilho)
    {
        await _context.FamiliaCaixilhos.AddAsync(familiaCaixilho);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var familia = await _context.FamiliaCaixilhos.FindAsync(id);
        if (familia != null)
        {
            _context.FamiliaCaixilhos.Remove(familia);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<FamiliaCaixilho>> GetAllAsync() =>
        await _context.FamiliaCaixilhos.Include(f => f.Obra).ToListAsync();

    public async Task<List<FamiliaCaixilho>> GetByObraIdAsync(int obraId) =>
        await _context.FamiliaCaixilhos
            .Where(f => f.IdObra == obraId)
            .Include(f => f.Obra)
            .ToListAsync();

    public async Task<int> CountByObraIdAsync(int obraId) =>
        await _context.FamiliaCaixilhos.CountAsync(f => f.IdObra == obraId);

    public async Task<FamiliaCaixilho?> GetByIdAsync(int id) =>
        await _context.FamiliaCaixilhos
            .Include(f => f.Obra)
            .FirstOrDefaultAsync(f => f.IdFamiliaCaixilho == id);

    public async Task UpdateAsync(FamiliaCaixilho familiaCaixilho)
    {
        var familiaTracked = await _context.FamiliaCaixilhos.FindAsync(familiaCaixilho.IdFamiliaCaixilho);
        if (familiaTracked == null)
        {
            throw new InvalidOperationException($"FamiliaCaixilho com ID {familiaCaixilho.IdFamiliaCaixilho} não encontrado.");
        }

        familiaTracked.DescricaoFamilia = familiaCaixilho.DescricaoFamilia;
        familiaTracked.StatusFamilia = familiaCaixilho.StatusFamilia;
        familiaTracked.PesoTotal = familiaCaixilho.PesoTotal;

        await _context.SaveChangesAsync();
    }

    public async Task<float> CalcularPesoTotalAsync(int familiaId)
    {
        var caixilhos = await _context.Caixilhos
            .Where(c => c.IdFamiliaCaixilho == familiaId)
            .ToListAsync();

        return caixilhos.Sum(c => c.PesoUnitario * c.Quantidade);
    }

    public async Task AtualizarPesoTotalAsync(int familiaId)
    {
        var familia = await _context.FamiliaCaixilhos.FindAsync(familiaId);
        if (familia != null)
        {
            familia.PesoTotal = (int)await CalcularPesoTotalAsync(familiaId);
            await _context.SaveChangesAsync();
        }
    }
}
