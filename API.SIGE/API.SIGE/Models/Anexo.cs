using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API.SIGE.Model;

[Table("Anexo")]
public class Anexo
{
    [Key]
    public int IdAnexo { get; set; }

    [Required]
    [StringLength(255)]
    public string NomeArquivo { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string CaminhoArquivo { get; set; } = string.Empty;

    [StringLength(100)]
    public string TipoArquivo { get; set; } = string.Empty;

    public long TamanhoBytes { get; set; }

    public DateTime DataUpload { get; set; }

    public TipoAnexo TipoAnexo { get; set; }

    public int? IdMedicao { get; set; }
    public int? IdProducaoFamilia { get; set; }

    [Required]
    public int IdUsuario { get; set; }

    [ForeignKey(nameof(IdMedicao))]
    public virtual Medicao? Medicao { get; set; }

    [ForeignKey(nameof(IdProducaoFamilia))]
    public virtual ProducaoFamilia? ProducaoFamilia { get; set; }

    [ForeignKey(nameof(IdUsuario))]
    public virtual Usuario Usuario { get; set; } = null!;

    [Required]
    public int IdEmpresa { get; set; }

    [JsonIgnore]
    [ForeignKey(nameof(IdEmpresa))]
    public virtual Empresa? Empresa { get; set; }
}
