using System.ComponentModel.DataAnnotations;

namespace API.SIGE.DTOs;

public class ProducaoFamiliaIniciarDto
{
    [Required]
    public int IdResponsavel { get; set; }

    public DateTime? DataEstimadaConclusao { get; set; }

    [StringLength(200)]
    public string? Descricao { get; set; }
}
