using API.SIGE.Data;
using SIGE.API.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace API.SIGE.Controllers;

[EnableCors("MyPolicy")]
[ApiController]
[Route("api/[controller]")]
public class HomeApiController : ControllerBase
{
    private readonly AppDbData _context;

    public HomeApiController(AppDbData context)
    {
        _context = context;
    }

    [HttpGet("dashboard")]
    public async Task<ActionResult> Dashboard()
    {
        var totalObras = await _context.Obras.CountAsync();
        var totalProducoes = await _context.Producoes.CountAsync();
        var totalCaixilhos = await _context.Caixilhos.CountAsync();

        return Ok(new
        {
            totalObras,
            totalProducoes,
            totalCaixilhos
        });
    }

    [HttpPost("importar-obras-xml")]
    public async Task<ActionResult> ImportarObrasXml([FromForm] List<IFormFile> arquivosXml)
    {
        if (arquivosXml == null || arquivosXml.Count == 0)
        {
            return BadRequest(new { success = false, message = "Nenhum arquivo XML enviado." });
        }

        var resultados = new List<string>();
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        foreach (var arquivoXml in arquivosXml)
        {
            try
            {
                using var stream = arquivoXml.OpenReadStream();
                using var reader = new StreamReader(stream, System.Text.Encoding.GetEncoding("windows-1252"));
                var xdoc = XDocument.Load(reader);

                var dadosObra = xdoc.Root?.Element("DADOS_OBRA");
                var enderecoObra = dadosObra?.Element("ENDERECO_OBRA");
                var dadosCliente = xdoc.Root?.Element("DADOS_CLIENTE");

                var obra = new Obra
                {
                    Nome = dadosObra?.Element("NOME")?.Value ?? "Obra sem nome",
                    Construtora = dadosCliente?.Element("NOME")?.Value ?? "Sem construtora",
                    Cnpj = dadosCliente?.Element("CNPJ_CPF")?.Value ?? "00000000000000",
                    Logradouro = enderecoObra?.Element("END_LOGR")?.Value ?? "Nao informado",
                    Nro = enderecoObra?.Element("END_NUMERO")?.Value ?? "0",
                    Bairro = enderecoObra?.Element("END_BAIRRO")?.Value ?? "Nao informado",
                    Cep = enderecoObra?.Element("END_CEP")?.Value ?? "00000-000",
                    Uf = enderecoObra?.Element("END_UF")?.Value ?? "SP",
                    IdUsuario = 3
                };

                _context.Obras.Add(obra);
                resultados.Add($"Arquivo '{arquivoXml.FileName}' importado com sucesso.");
            }
            catch (Exception ex)
            {
                resultados.Add($"Arquivo '{arquivoXml.FileName}' com erro: {ex.Message}");
            }
        }

        await _context.SaveChangesAsync();
        return Ok(new { success = true, resultados });
    }
}
