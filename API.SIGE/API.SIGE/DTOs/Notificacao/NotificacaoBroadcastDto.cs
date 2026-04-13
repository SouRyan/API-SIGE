using API.SIGE.Model;

namespace API.SIGE.DTOs;

public class NotificacaoBroadcastDto
{
    public string Titulo { get; set; } = string.Empty;
    public string Mensagem { get; set; } = string.Empty;
    public TipoNotificacao TipoNotificacao { get; set; }
    public int? IdObra { get; set; }
    public TipoCargo TipoCargo { get; set; }
}
