using System.ComponentModel.DataAnnotations;

namespace API.SIGE.DTOs;

public class AprovarSolicitacaoDto
{
    [Required]
    [StringLength(50)]
    public string SenhaUsuarioAdmin { get; set; } = string.Empty;

    [Required]
    public int IdTipoUsuarioAdmin { get; set; }
}
