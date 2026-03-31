using API.SIGE.Model;

namespace API.SIGE.DTOs;

public class CargoResponseDto
{
    public int IdCargo { get; set; }
    public TipoCargo TipoCargo { get; set; }
    public string DescricaoCargo { get; set; } = string.Empty;
}
