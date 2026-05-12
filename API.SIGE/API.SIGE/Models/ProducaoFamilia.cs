using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API.SIGE.Model;

[Table("ProducaoFamilia")]
public class ProducaoFamilia
{
    [Key]
    public int IdProducaoFamilia { get; set; }

    [Required]
    public int IdFamiliaCaixilho { get; set; }

    [Required]
    public int IdResponsavel { get; set; }

    public StatusAtividade Status { get; set; } = StatusAtividade.NaoIniciada;

    public DateTime? DataInicio { get; set; }
    public DateTime? DataEstimadaConclusao { get; set; }
    public DateTime? DataConclusao { get; set; }

    [StringLength(200)]
    public string? Descricao { get; set; }

    [StringLength(500)]
    public string? Observacoes { get; set; }

    [ForeignKey(nameof(IdFamiliaCaixilho))]
    public virtual FamiliaCaixilho FamiliaCaixilho { get; set; } = null!;

    [ForeignKey(nameof(IdResponsavel))]
    public virtual Usuario Responsavel { get; set; } = null!;

    [Required]
    public int IdEmpresa { get; set; }

    [JsonIgnore]
    [ForeignKey(nameof(IdEmpresa))]
    public virtual Empresa? Empresa { get; set; }
}
