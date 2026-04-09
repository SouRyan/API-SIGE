using API.SIGE.Data;
using API.SIGE.Interfaces.Repositories;
using API.SIGE.Model;
using Microsoft.EntityFrameworkCore;

namespace API.SIGE.Repositories;

public class ObraRepository : IObraRepository
{
    private readonly AppDbData _context;

    public ObraRepository(AppDbData context)
    {
        _context = context;
    }

    public async Task AddAsync(Obra obra)
    {
        await _context.Obras.AddAsync(obra);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var obra = await _context.Obras.FindAsync(id);
        if (obra != null)
        {
            _context.Obras.Remove(obra);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Obra>> GetAllAsync() =>
        await _context.Obras
            .Include(o => o.Usuario)
            .ToListAsync();

    public async Task<List<Obra>> GetAllFinalizadosAsync() =>
        await _context.Obras
            .Where(o => o.Finalizado)
            .Include(o => o.Usuario)
            .ToListAsync();

    public async Task<List<Obra>> GetAllNaoFinalizadosAsync() =>
        await _context.Obras
            .Where(o => o.Finalizado == false)
            .Include(o => o.Usuario)
            .ToListAsync();

    public async Task<List<Obra>> GetByStatusAsync(StatusObra status) =>
        await _context.Obras
            .Where(o => o.StatusObra == status)
            .Include(o => o.Usuario)
            .ToListAsync();

    public async Task<Obra?> GetById(int id) =>
        await _context.Obras
            .Include(o => o.Usuario)
            .FirstOrDefaultAsync(o => o.IdObra == id);

    public async Task UpdateAsync(Obra obra)
    {
        _context.Obras.Update(obra);
        await _context.SaveChangesAsync();
    }
}
