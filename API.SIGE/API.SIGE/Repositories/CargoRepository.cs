using API.SIGE.Data;
using API.SIGE.Interfaces.Repositories;
using API.SIGE.Model;
using Microsoft.EntityFrameworkCore;

namespace API.SIGE.Repositories;

public class CargoRepository : ICargoRepository
{
    private readonly AppDbData _context;

    public CargoRepository(AppDbData context)
    {
        _context = context;
    }

    public async Task<List<Cargo>> GetAllAsync() =>
        await _context.Cargos.AsNoTracking().ToListAsync();

    public async Task<Cargo?> GetByIdAsync(int id) =>
        await _context.Cargos.FindAsync(id);

    public async Task<Cargo?> GetByTipoAsync(TipoCargo tipo) =>
        await _context.Cargos.AsNoTracking().FirstOrDefaultAsync(c => c.TipoCargo == tipo);

    public async Task AddAsync(Cargo cargo)
    {
        await _context.Cargos.AddAsync(cargo);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Cargo cargo)
    {
        _context.Cargos.Update(cargo);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Cargos.FindAsync(id);
        if (entity != null)
        {
            _context.Cargos.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
