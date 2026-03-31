using API.SIGE.Model;

namespace API.SIGE.Interfaces.Repositories;

public interface INotificacaoRepository
{
    Task<List<Notificacao>> GetByUsuarioIdAsync(int idUsuario);
    Task<List<Notificacao>> GetNaoLidasAsync(int idUsuario);
    Task<Notificacao?> GetByIdAsync(int id);
    Task AddAsync(Notificacao notificacao);
    Task UpdateAsync(Notificacao notificacao);
    Task DeleteAsync(int id);
}
