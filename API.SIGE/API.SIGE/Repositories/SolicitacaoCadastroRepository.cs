using API.SIGE.Data;
using API.SIGE.Interfaces.Repositories;
using API.SIGE.Model;
using Microsoft.EntityFrameworkCore;

namespace API.SIGE.Repositories;

public class SolicitacaoCadastroRepository : ISolicitacaoCadastroRepository
{
    private readonly AppDbData _context;

    public SolicitacaoCadastroRepository(AppDbData context)
    {
        _context = context;
    }

    public async Task<List<SolicitacaoCadastro>> GetAllAsync() =>
        await _context.SolicitacoesCadastro
            .OrderByDescending(s => s.DataSolicitacao)
            .ToListAsync();

    public async Task<List<SolicitacaoCadastro>> GetPendentesAsync() =>
        await _context.SolicitacoesCadastro
            .Where(s => s.Status == StatusSolicitacaoCadastro.Pendente)
            .OrderBy(s => s.DataSolicitacao)
            .ToListAsync();

    public async Task<SolicitacaoCadastro?> GetByIdAsync(int id) =>
        await _context.SolicitacoesCadastro.FindAsync(id);

    public async Task<SolicitacaoCadastro?> GetByCnpjAsync(string cnpj) =>
        await _context.SolicitacoesCadastro
            .FirstOrDefaultAsync(s => s.Cnpj == cnpj && s.Status == StatusSolicitacaoCadastro.Pendente);

    public async Task AddAsync(SolicitacaoCadastro solicitacao)
    {
        await _context.SolicitacoesCadastro.AddAsync(solicitacao);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(SolicitacaoCadastro solicitacao)
    {
        _context.SolicitacoesCadastro.Update(solicitacao);
        await _context.SaveChangesAsync();
    }
}
