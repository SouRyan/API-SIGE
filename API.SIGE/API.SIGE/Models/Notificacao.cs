using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API.SIGE.Model;

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

    [Required]
    public int IdEmpresa { get; set; }

    [JsonIgnore]
    [ForeignKey(nameof(IdEmpresa))]
    public virtual Empresa? Empresa { get; set; }
}
