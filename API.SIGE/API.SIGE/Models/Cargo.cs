using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.SIGE.Models;

[Table("Cargo")]
public class Cargo
{
    [Key]
    public int IdCargo { get; set; }

    public TipoCargo TipoCargo { get; set; }

    [Required]
    [StringLength(100)]
    public string DescricaoCargo { get; set; } = string.Empty;

    public ICollection<UsuarioCargo>? UsuarioCargos { get; set; }
}
