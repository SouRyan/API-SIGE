using API.SIGE.DTOs;
using API.SIGE.Models;

namespace API.SIGE.Interfaces.Services;

public interface INotificacaoService
{
    Task<List<NotificacaoResponseDto>> GetByUsuarioIdAsync(int idUsuario);
    Task MarcarLidaAsync(int idNotificacao);
    Task CriarAsync(int idDestino, string titulo, string mensagem, TipoNotificacao tipo, int? idObra);
}
