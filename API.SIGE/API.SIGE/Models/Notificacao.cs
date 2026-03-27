using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SIGE.API.Models;

namespace API.SIGE.Models;

[Table("Notificacao")]
public class Notificacao
{
    [Key]
    public int IdNotificacao { get; set; }

    [Required]
    public int IdUsuarioDestino { get; set; }

    [Required]
    [StringLength(200)]
    public string Titulo { get; set; } = string.Empty;

    [Required]
    [StringLength(1000)]
    public string Mensagem { get; set; } = string.Empty;

    public bool Lida { get; set; }

    public DateTime DataCriacao { get; set; }

    public TipoNotificacao TipoNotificacao { get; set; }

    public int? IdObra { get; set; }

    [ForeignKey(nameof(IdUsuarioDestino))]
    public virtual Usuario UsuarioDestino { get; set; } = null!;

    [ForeignKey(nameof(IdObra))]
    public virtual Obra? Obra { get; set; }
}
