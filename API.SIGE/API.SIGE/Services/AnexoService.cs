using API.SIGE.DTOs;
using API.SIGE.Interfaces.Repositories;
using API.SIGE.Interfaces.Services;
using API.SIGE.Model;
using Microsoft.AspNetCore.Hosting;

namespace API.SIGE.Services;

public class AnexoService : IAnexoService
{
    private readonly IAnexoRepository _anexoRepository;
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public AnexoService(
        IAnexoRepository anexoRepository,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        _anexoRepository = anexoRepository;
        _configuration = configuration;
        _environment = environment;
    }

    public async Task<AnexoResponseDto> UploadAsync(AnexoUploadDto dto)
    {
        if (dto.Arquivo == null || dto.Arquivo.Length == 0)
            throw new InvalidOperationException("Arquivo obrigatório.");

        var basePath = _configuration["FileStorage:BasePath"];
        if (string.IsNullOrWhiteSpace(basePath))
            basePath = Path.Combine(_environment.ContentRootPath, "uploads");

        var maxMb = _configuration.GetValue("FileStorage:MaxFileSizeMB", 10);
        var maxBytes = maxMb * 1024L * 1024L;
        if (dto.Arquivo.Length > maxBytes)
            throw new InvalidOperationException($"Arquivo excede o tamanho máximo de {maxMb} MB.");

        var allowed = _configuration["FileStorage:AllowedExtensions"] ?? ".pdf,.jpg,.jpeg,.png,.xlsx,.docx";
        var ext = Path.GetExtension(dto.Arquivo.FileName).ToLowerInvariant();
        var allowedSet = allowed.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(e => e.StartsWith('.') ? e.ToLowerInvariant() : "." + e.ToLowerInvariant());
        if (!allowedSet.Contains(ext))
            throw new InvalidOperationException("Extensão de arquivo não permitida.");

        Directory.CreateDirectory(basePath);
        var storedName = $"{Guid.NewGuid():N}{ext}";
        var fullPath = Path.Combine(basePath, storedName);
        await using (var stream = File.Create(fullPath))
        {
            await dto.Arquivo.CopyToAsync(stream);
        }

        var anexo = new Anexo
        {
            NomeArquivo = dto.Arquivo.FileName,
            CaminhoArquivo = fullPath,
            TipoArquivo = dto.Arquivo.ContentType,
            TamanhoBytes = dto.Arquivo.Length,
            DataUpload = DateTime.UtcNow,
            TipoAnexo = dto.TipoAnexo,
            IdMedicao = dto.IdMedicao,
            IdProducaoFamilia = dto.IdProducaoFamilia,
            IdUsuario = dto.IdUsuario
        };
        await _anexoRepository.AddAsync(anexo);

        var loaded = await _anexoRepository.GetByIdAsync(anexo.IdAnexo);
        return Map(loaded!);
    }

    public async Task<List<AnexoResponseDto>> GetByMedicaoIdAsync(int medicaoId)
    {
        var list = await _anexoRepository.GetByMedicaoIdAsync(medicaoId);
        return list.Select(Map).ToList();
    }

    public async Task<List<AnexoResponseDto>> GetByProducaoFamiliaIdAsync(int producaoFamiliaId)
    {
        var list = await _anexoRepository.GetByProducaoFamiliaIdAsync(producaoFamiliaId);
        return list.Select(Map).ToList();
    }

    public async Task DeleteAsync(int id)
    {
        var anexo = await _anexoRepository.GetByIdAsync(id)
            ?? throw new InvalidOperationException("Anexo não encontrado.");
        if (File.Exists(anexo.CaminhoArquivo))
            File.Delete(anexo.CaminhoArquivo);
        await _anexoRepository.DeleteAsync(id);
    }

    private static AnexoResponseDto Map(Anexo a) => new()
    {
        IdAnexo = a.IdAnexo,
        NomeArquivo = a.NomeArquivo,
        CaminhoArquivo = a.CaminhoArquivo,
        TipoArquivo = a.TipoArquivo,
        TamanhoBytes = a.TamanhoBytes,
        DataUpload = a.DataUpload,
        TipoAnexo = a.TipoAnexo,
        IdMedicao = a.IdMedicao,
        IdProducaoFamilia = a.IdProducaoFamilia,
        IdUsuario = a.IdUsuario,
        NomeUsuario = a.Usuario?.NomeUsuario
    };
}
