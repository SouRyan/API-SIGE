using API.SIGE.Data;
using API.SIGE.Interfaces.Repositories;
using API.SIGE.Models;
using Microsoft.EntityFrameworkCore;

namespace API.SIGE.Repositories;

public class CaixilhoRepository : ICaixilhoRepository
{
    private readonly AppDbData _context;
    private readonly IFamiliaCaixilhoRepository _familiaRepository;

    public CaixilhoRepository(AppDbData context, IFamiliaCaixilhoRepository familiaRepository)
    {
        _context = context;
        _familiaRepository = familiaRepository;
    }

    public async Task AddAsync(Caixilho caixilho)
    {
        await _context.Caixilhos.AddAsync(caixilho);
        await _context.SaveChangesAsync();
        await _familiaRepository.AtualizarPesoTotalAsync(caixilho.IdFamiliaCaixilho);
    }

    public async Task DeleteAsync(int id)
    {
        var caixilho = await _context.Caixilhos.FindAsync(id);
        if (caixilho != null)
        {
            var familiaId = caixilho.IdFamiliaCaixilho;
            _context.Caixilhos.Remove(caixilho);
            await _context.SaveChangesAsync();
            await _familiaRepository.AtualizarPesoTotalAsync(familiaId);
        }
    }

    public async Task<List<Caixilho>> GetAllAsync() =>
        await _context.Caixilhos
            .Include(c => c.Obra)
            .Include(c => c.FamiliaCaixilho)
            .ToListAsync();

    public async Task<Caixilho?> GetById(int id) =>
        await _context.Caixilhos
            .Include(c => c.Obra)
            .Include(c => c.FamiliaCaixilho)
            .FirstOrDefaultAsync(c => c.IdCaixilho == id);

    public async Task<Caixilho?> GetByFamilia(Caixilho caixilho) =>
        await _context.Caixilhos
            .Include(c => c.Obra)
            .Include(c => c.FamiliaCaixilho)
            .FirstOrDefaultAsync(c => c.IdFamiliaCaixilho == caixilho.IdFamiliaCaixilho);

    public async Task<int> CountByFamiliaIdAsync(int familiaId) =>
        await _context.Caixilhos.CountAsync(c => c.IdFamiliaCaixilho == familiaId);

    public async Task UpdateAsync(Caixilho caixilho)
    {
        var caixilhoOriginal = await _context.Caixilhos.AsNoTracking()
            .FirstOrDefaultAsync(c => c.IdCaixilho == caixilho.IdCaixilho);
        var familiaAnterior = caixilhoOriginal?.IdFamiliaCaixilho;
        var familiaAtual = caixilho.IdFamiliaCaixilho;

        var caixilhoTracked = await _context.Caixilhos.FindAsync(caixilho.IdCaixilho);
        if (caixilhoTracked == null)
        {
            throw new InvalidOperationException($"Caixilho com ID {caixilho.IdCaixilho} não encontrado.");
        }

        caixilhoTracked.NomeCaixilho = caixilho.NomeCaixilho;
        caixilhoTracked.Largura = caixilho.Largura;
        caixilhoTracked.Altura = caixilho.Altura;
        caixilhoTracked.Quantidade = caixilho.Quantidade;
        caixilhoTracked.PesoUnitario = caixilho.PesoUnitario;
        caixilhoTracked.ObraId = caixilho.ObraId;
        caixilhoTracked.IdFamiliaCaixilho = caixilho.IdFamiliaCaixilho;
        caixilhoTracked.Liberado = caixilho.Liberado;
        caixilhoTracked.DataLiberacao = caixilho.DataLiberacao;
        caixilhoTracked.StatusProducao = caixilho.StatusProducao;
        caixilhoTracked.Observacoes = caixilho.Observacoes;

        await _context.SaveChangesAsync();
        await _familiaRepository.AtualizarPesoTotalAsync(familiaAtual);
        if (familiaAnterior.HasValue && familiaAnterior.Value != familiaAtual)
        {
            await _familiaRepository.AtualizarPesoTotalAsync(familiaAnterior.Value);
        }
    }
}
