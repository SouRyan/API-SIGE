using System.ComponentModel.DataAnnotations;

namespace API.SIGE.DTOs;

public class RecusarSolicitacaoDto
{
    [Required]
    [StringLength(500)]
    public string MotivoRecusa { get; set; } = string.Empty;
}
