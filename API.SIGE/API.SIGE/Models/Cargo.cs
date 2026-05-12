using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API.SIGE.Model;

[Table("Cargo")]
public class Cargo
{
    [Key]
    public int IdCargo { get; set; }

    public TipoCargo TipoCargo { get; set; }

    [Required]
    [StringLength(100)]
    public string DescricaoCargo { get; set; } = string.Empty;

    public ICollection<Usuario>? Usuarios { get; set; }

    [Required]
    public int IdEmpresa { get; set; }

    [JsonIgnore]
    [ForeignKey(nameof(IdEmpresa))]
    public virtual Empresa? Empresa { get; set; }
}
