using API.SIGE.Data;
using API.SIGE.Interfaces.Repositories;
using API.SIGE.Models;
using Microsoft.EntityFrameworkCore;

namespace API.SIGE.Repositories;

public class UsuarioCargoRepository : IUsuarioCargoRepository
{
    private readonly AppDbData _context;

    public UsuarioCargoRepository(AppDbData context)
    {
        _context = context;
    }

    public async Task<List<UsuarioCargo>> GetByUsuarioIdAsync(int idUsuario) =>
        await _context.UsuarioCargos
            .Include(uc => uc.Cargo)
            .Where(uc => uc.IdUsuario == idUsuario)
            .ToListAsync();

    public async Task<UsuarioCargo?> GetByIdAsync(int idUsuarioCargo) =>
        await _context.UsuarioCargos.FindAsync(idUsuarioCargo);

    public async Task AddAsync(UsuarioCargo usuarioCargo)
    {
        await _context.UsuarioCargos.AddAsync(usuarioCargo);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int idUsuarioCargo)
    {
        var entity = await _context.UsuarioCargos.FindAsync(idUsuarioCargo);
        if (entity != null)
        {
            _context.UsuarioCargos.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int idUsuario, int idCargo) =>
        await _context.UsuarioCargos.AnyAsync(uc => uc.IdUsuario == idUsuario && uc.IdCargo == idCargo);
}
