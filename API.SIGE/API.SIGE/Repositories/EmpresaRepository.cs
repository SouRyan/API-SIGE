using API.SIGE.Data;
using API.SIGE.Interfaces.Repositories;
using API.SIGE.Model;
using Microsoft.EntityFrameworkCore;

namespace API.SIGE.Repositories;

public class EmpresaRepository : IEmpresaRepository
{
    private readonly AppDbData _context;

    public EmpresaRepository(AppDbData context)
    {
        _context = context;
    }

    public async Task<List<Empresa>> GetAllAsync()
    {
        return await _context.Empresas.Where(e => e.Ativo).ToListAsync();
    }

    public async Task<Empresa?> GetById(int id)
    {
        return await _context.Empresas.FindAsync(id);
    }

    public async Task<Empresa?> GetInativados(int id)
    {
        return await _context.Empresas
            .FirstOrDefaultAsync(e => e.IdEmpresa == id && !e.Ativo);
    }

    public async Task AddAsync(Empresa empresa)
    {
        await _context.Empresas.AddAsync(empresa);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Empresa empresa)
    {
        _context.Empresas.Update(empresa);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var empresa = await _context.Empresas.FindAsync(id);
        if (empresa != null)
        {
            _context.Empresas.Remove(empresa);
            await _context.SaveChangesAsync();
        }
    }
}
