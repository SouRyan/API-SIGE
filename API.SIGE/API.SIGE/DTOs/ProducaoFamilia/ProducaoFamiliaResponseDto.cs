using API.SIGE.Models;

namespace API.SIGE.DTOs;

public class ProducaoFamiliaResponseDto
{
    public int IdProducaoFamilia { get; set; }
    public int IdFamiliaCaixilho { get; set; }
    public int IdResponsavel { get; set; }
    public string? NomeResponsavel { get; set; }
    public StatusAtividade Status { get; set; }
    public DateTime? DataInicio { get; set; }
    public DateTime? DataEstimadaConclusao { get; set; }
    public DateTime? DataConclusao { get; set; }
    public string? Descricao { get; set; }
    public string? Observacoes { get; set; }
}
