using API.SIGE.Data;
using API.SIGE.Interfaces.Repositories;
using API.SIGE.Model;
using Microsoft.EntityFrameworkCore;

namespace API.SIGE.Repositories;

public class AnexoRepository : IAnexoRepository
{
    private readonly AppDbData _context;

    public AnexoRepository(AppDbData context)
    {
        _context = context;
    }

    public async Task<List<Anexo>> GetAllAsync() =>
        await _context.Anexos.Include(a => a.Usuario).ToListAsync();

    public async Task<Anexo?> GetByIdAsync(int id) =>
        await _context.Anexos.Include(a => a.Usuario).FirstOrDefaultAsync(a => a.IdAnexo == id);

    public async Task<List<Anexo>> GetByMedicaoIdAsync(int medicaoId) =>
        await _context.Anexos
            .Where(a => a.IdMedicao == medicaoId)
            .Include(a => a.Usuario)
            .ToListAsync();

    public async Task<List<Anexo>> GetByProducaoFamiliaIdAsync(int producaoFamiliaId) =>
        await _context.Anexos
            .Where(a => a.IdProducaoFamilia == producaoFamiliaId)
            .Include(a => a.Usuario)
            .ToListAsync();

    public async Task AddAsync(Anexo anexo)
    {
        await _context.Anexos.AddAsync(anexo);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Anexos.FindAsync(id);
        if (entity != null)
        {
            _context.Anexos.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
