using API.SIGE.Models;

namespace API.SIGE.DTOs;

public class AnexoResponseDto
{
    public int IdAnexo { get; set; }
    public string NomeArquivo { get; set; } = string.Empty;
    public string CaminhoArquivo { get; set; } = string.Empty;
    public string TipoArquivo { get; set; } = string.Empty;
    public long TamanhoBytes { get; set; }
    public DateTime DataUpload { get; set; }
    public TipoAnexo TipoAnexo { get; set; }
    public int? IdMedicao { get; set; }
    public int? IdProducaoFamilia { get; set; }
    public int IdUsuario { get; set; }
    public string? NomeUsuario { get; set; }
}
