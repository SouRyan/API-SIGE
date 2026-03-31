using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.SIGE.Model;

[Table("Medicao")]
public class Medicao
{
    [Key]
    public int IdMedicao { get; set; }

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
}
