using API.SIGE.Data;
using API.SIGE.Interfaces.Repositories;
using API.SIGE.Model;
using Microsoft.EntityFrameworkCore;

namespace API.SIGE.Repositories;

public class SolicitacaoClienteRepository : ISolicitacaoClienteRepository
{
    private readonly AppDbData _context;

    public SolicitacaoClienteRepository(AppDbData context)
    {
        _context = context;
    }

    public async Task<List<SolicitacaoCliente>> GetByClienteIdAsync(int clienteId)
    {
        return await _context.SolicitacoesCliente
            .Include(s => s.Caixilho)
            .Include(s => s.Cliente)
            .Where(s => s.IdCliente == clienteId)
            .OrderByDescending(s => s.DataSolicitacao)
            .ToListAsync();
    }

    public async Task<List<SolicitacaoCliente>> GetByFamiliaIdAsync(int familiaId)
    {
        return await _context.SolicitacoesCliente
            .Include(s => s.Caixilho)
            .Include(s => s.Cliente)
            .Where(s => s.Caixilho!.IdFamiliaCaixilho == familiaId)
            .OrderByDescending(s => s.Prioridade)
            .ToListAsync();
    }

    public async Task<SolicitacaoCliente?> GetByIdAsync(int id)
    {
        return await _context.SolicitacoesCliente
            .Include(s => s.Caixilho)
            .Include(s => s.Cliente)
            .FirstOrDefaultAsync(s => s.IdSolicitacao == id);
    }

    public async Task<SolicitacaoCliente?> GetByCaixilhoEClienteAsync(int caixilhoId, int clienteId)
    {
        return await _context.SolicitacoesCliente
            .FirstOrDefaultAsync(s => s.IdCaixilho == caixilhoId && s.IdCliente == clienteId);
    }

    public async Task AddAsync(SolicitacaoCliente solicitacao)
    {
        _context.SolicitacoesCliente.Add(solicitacao);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(SolicitacaoCliente solicitacao)
    {
        _context.SolicitacoesCliente.Update(solicitacao);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var solicitacao = await _context.SolicitacoesCliente.FindAsync(id);
        if (solicitacao != null)
        {
            _context.SolicitacoesCliente.Remove(solicitacao);
            await _context.SaveChangesAsync();
        }
    }
}