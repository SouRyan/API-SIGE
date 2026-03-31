using API.SIGE.Model;
using Microsoft.AspNetCore.Http;

namespace API.SIGE.DTOs;

public class AnexoUploadDto
{
    public IFormFile? Arquivo { get; set; }
    public TipoAnexo TipoAnexo { get; set; }
    public int? IdMedicao { get; set; }
    public int? IdProducaoFamilia { get; set; }
    public int IdUsuario { get; set; }
}
