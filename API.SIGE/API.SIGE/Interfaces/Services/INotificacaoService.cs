using API.SIGE.DTOs;
using API.SIGE.Model;

namespace API.SIGE.Interfaces.Services;

public interface INotificacaoService
{
    Task<List<NotificacaoResponseDto>> GetByUsuarioIdAsync(int idUsuario);
    Task<int> GetNaoLidasCountAsync(int idUsuario);
    Task MarcarLidaAsync(int idNotificacao);
    Task ApagarAsync(int idNotificacao);
    Task<int> ApagarLidasAsync(int idUsuario);
    Task CriarAsync(int idDestino, string titulo, string mensagem, TipoNotificacao tipo, int? idObra);
    Task BroadcastAsync(string titulo, string mensagem, TipoNotificacao tipo, int? idObra, TipoCargo tipoCargo);
}
