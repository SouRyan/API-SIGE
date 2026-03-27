using API.SIGE.DTOs;
using API.SIGE.Interfaces.Repositories;
using API.SIGE.Interfaces.Services;
using API.SIGE.Models;
using SIGE.API.Models;

namespace API.SIGE.Services;

public class FamiliaCaixilhoService : IFamiliaCaixilhoService
{
    private const int MaxFamiliasPorObra = 10;

    private readonly IFamiliaCaixilhoRepository _familiaRepository;
    private readonly IObraRepository _obraRepository;
    private readonly ICaixilhoRepository _caixilhoRepository;

    public FamiliaCaixilhoService(
        IFamiliaCaixilhoRepository familiaRepository,
        IObraRepository obraRepository,
        ICaixilhoRepository caixilhoRepository)
    {
        _familiaRepository = familiaRepository;
        _obraRepository = obraRepository;
        _caixilhoRepository = caixilhoRepository;
    }

    public async Task<List<FamiliaCaixilhoResponseDto>> GetAllAsync()
    {
        var list = await _familiaRepository.GetAllAsync();
        var result = new List<FamiliaCaixilhoResponseDto>();
        foreach (var f in list)
            result.Add(await MapAsync(f));
        return result;
    }

    public async Task<List<FamiliaCaixilhoResponseDto>> GetByObraIdAsync(int obraId)
    {
        var list = await _familiaRepository.GetByObraIdAsync(obraId);
        var result = new List<FamiliaCaixilhoResponseDto>();
        foreach (var f in list)
            result.Add(await MapAsync(f));
        return result;
    }

    public async Task<FamiliaCaixilhoResponseDto?> GetByIdAsync(int id)
    {
        var f = await _familiaRepository.GetByIdAsync(id);
        return f == null ? null : await MapAsync(f);
    }

    public async Task<FamiliaCaixilhoResponseDto> CreateAsync(FamiliaCaixilhoCreateDto dto)
    {
        _ = await _obraRepository.GetById(dto.IdObra)
            ?? throw new InvalidOperationException("Obra não encontrada.");

        if (await _familiaRepository.CountByObraIdAsync(dto.IdObra) >= MaxFamiliasPorObra)
            throw new InvalidOperationException($"Limite de {MaxFamiliasPorObra} famílias por obra atingido.");

        var familia = new FamiliaCaixilho
        {
            DescricaoFamilia = dto.DescricaoFamilia,
            IdObra = dto.IdObra,
            PesoTotal = 0,
            StatusFamilia = StatusFamilia.Pendente
        };
        await _familiaRepository.AddAsync(familia);

        var created = await _familiaRepository.GetByIdAsync(familia.IdFamiliaCaixilho);
        return await MapAsync(created!);
    }

    public async Task UpdateAsync(int id, FamiliaCaixilhoUpdateDto dto)
    {
        var familia = await _familiaRepository.GetByIdAsync(id)
            ?? throw new InvalidOperationException("Família não encontrada.");
        familia.DescricaoFamilia = dto.DescricaoFamilia;
        await _familiaRepository.UpdateAsync(familia);
    }

    public async Task DeleteAsync(int id) => await _familiaRepository.DeleteAsync(id);

    public async Task<int> RecalcularPesosAsync()
    {
        var familias = await _familiaRepository.GetAllAsync();
        foreach (var f in familias)
            await _familiaRepository.AtualizarPesoTotalAsync(f.IdFamiliaCaixilho);
        return familias.Count;
    }

    private async Task<FamiliaCaixilhoResponseDto> MapAsync(FamiliaCaixilho f)
    {
        var qtd = await _caixilhoRepository.CountByFamiliaIdAsync(f.IdFamiliaCaixilho);
        return new FamiliaCaixilhoResponseDto
        {
            IdFamiliaCaixilho = f.IdFamiliaCaixilho,
            DescricaoFamilia = f.DescricaoFamilia,
            PesoTotal = f.PesoTotal,
            IdObra = f.IdObra,
            NomeObra = f.Obra?.Nome,
            StatusFamilia = f.StatusFamilia,
            QuantidadeCaixilhos = qtd
        };
    }
}
