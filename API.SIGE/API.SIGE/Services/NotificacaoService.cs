using API.SIGE.DTOs;
using API.SIGE.Interfaces.Repositories;
using API.SIGE.Interfaces.Services;
using API.SIGE.Model;

namespace API.SIGE.Services;

public class NotificacaoService : INotificacaoService
{
    private readonly INotificacaoRepository _notificacaoRepository;
    private readonly IUsuarioRepository _usuarioRepository;

    public NotificacaoService(INotificacaoRepository notificacaoRepository, IUsuarioRepository usuarioRepository)
    {
        _notificacaoRepository = notificacaoRepository;
        _usuarioRepository = usuarioRepository;
    }

    public async Task<List<NotificacaoResponseDto>> GetByUsuarioIdAsync(int idUsuario)
    {
        var lista = await _notificacaoRepository.GetByUsuarioIdAsync(idUsuario);
        return lista.Select(Map).ToList();
    }

    public async Task<int> GetNaoLidasCountAsync(int idUsuario)
    {
        var lista = await _notificacaoRepository.GetNaoLidasAsync(idUsuario);
        return lista.Count;
    }

    public async Task BroadcastAsync(string titulo, string mensagem, TipoNotificacao tipo, int? idObra, TipoCargo tipoCargo)
    {
        var usuarios = await _usuarioRepository.GetByCargoAsync(tipoCargo);
        foreach (var u in usuarios)
        {
            await CriarAsync(u.IdUsuario, titulo, mensagem, tipo, idObra);
        }
    }

    public async Task MarcarLidaAsync(int idNotificacao)
    {
        var n = await _notificacaoRepository.GetByIdAsync(idNotificacao)
            ?? throw new InvalidOperationException("Notificação não encontrada.");
        n.Lida = true;
        await _notificacaoRepository.UpdateAsync(n);
    }

    public async Task CriarAsync(int idDestino, string titulo, string mensagem, TipoNotificacao tipo, int? idObra)
    {
        var n = new Notificacao
        {
            IdUsuarioDestino = idDestino,
            Titulo = titulo,
            Mensagem = mensagem,
            TipoNotificacao = tipo,
            IdObra = idObra,
            Lida = false,
            DataCriacao = DateTime.UtcNow
        };
        await _notificacaoRepository.AddAsync(n);
    }

    private static NotificacaoResponseDto Map(Notificacao n) => new()
    {
        IdNotificacao = n.IdNotificacao,
        IdUsuarioDestino = n.IdUsuarioDestino,
        Titulo = n.Titulo,
        Mensagem = n.Mensagem,
        Lida = n.Lida,
        DataCriacao = n.DataCriacao,
        TipoNotificacao = n.TipoNotificacao,
        IdObra = n.IdObra
    };
}
