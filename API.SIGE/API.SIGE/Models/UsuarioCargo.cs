using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SIGE.API.Models;

namespace API.SIGE.Models;

[Table("UsuarioCargo")]
public class UsuarioCargo
{
    [Key]
    public int IdUsuarioCargo { get; set; }

    public int IdUsuario { get; set; }
    public int IdCargo { get; set; }

    [ForeignKey(nameof(IdUsuario))]
    public virtual Usuario Usuario { get; set; } = null!;

    [ForeignKey(nameof(IdCargo))]
    public virtual Cargo Cargo { get; set; } = null!;
}
