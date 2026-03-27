using API.SIGE.Data;
using API.SIGE.Interfaces.Repositories;
using API.SIGE.Models;
using Microsoft.EntityFrameworkCore;

namespace API.SIGE.Repositories;

public class NotificacaoRepository : INotificacaoRepository
{
    private readonly AppDbData _context;

    public NotificacaoRepository(AppDbData context)
    {
        _context = context;
    }

    public async Task<List<Notificacao>> GetByUsuarioIdAsync(int idUsuario) =>
        await _context.Notificacoes
            .Where(n => n.IdUsuarioDestino == idUsuario)
            .OrderByDescending(n => n.DataCriacao)
            .ToListAsync();

    public async Task<List<Notificacao>> GetNaoLidasAsync(int idUsuario) =>
        await _context.Notificacoes
            .Where(n => n.IdUsuarioDestino == idUsuario && !n.Lida)
            .OrderByDescending(n => n.DataCriacao)
            .ToListAsync();

    public async Task<Notificacao?> GetByIdAsync(int id) =>
        await _context.Notificacoes.FindAsync(id);

    public async Task AddAsync(Notificacao notificacao)
    {
        await _context.Notificacoes.AddAsync(notificacao);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Notificacao notificacao)
    {
        _context.Notificacoes.Update(notificacao);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Notificacoes.FindAsync(id);
        if (entity != null)
        {
            _context.Notificacoes.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
