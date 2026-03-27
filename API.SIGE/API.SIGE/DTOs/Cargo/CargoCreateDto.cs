using System.ComponentModel.DataAnnotations;
using API.SIGE.Models;

namespace API.SIGE.DTOs;

public class CargoCreateDto
{
    public TipoCargo TipoCargo { get; set; }

    [Required]
    [StringLength(100)]
    public string DescricaoCargo { get; set; } = string.Empty;
}
