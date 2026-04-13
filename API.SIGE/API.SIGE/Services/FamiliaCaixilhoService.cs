using API.SIGE.DTOs;
using API.SIGE.Interfaces.Repositories;
using API.SIGE.Interfaces.Services;
using API.SIGE.Model;

namespace API.SIGE.Services;

public class FamiliaCaixilhoService : IFamiliaCaixilhoService
{
    private const int MaxFamiliasPorObra = 10;

    private readonly IFamiliaCaixilhoRepository _familiaRepository;
    private readonly IObraRepository _obraRepository;
    private readonly ICaixilhoRepository _caixilhoRepository;
    private readonly IObraService _obraService;

    public FamiliaCaixilhoService(
        IFamiliaCaixilhoRepository familiaRepository,
        IObraRepository obraRepository,
        ICaixilhoRepository caixilhoRepository,
        IObraService obraService)
    {
        _familiaRepository = familiaRepository;
        _obraRepository = obraRepository;
        _caixilhoRepository = caixilhoRepository;
        _obraService = obraService;
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
        if (dto.StatusFamilia.HasValue && Enum.IsDefined(typeof(StatusFamilia), dto.StatusFamilia.Value))
            familia.StatusFamilia = (StatusFamilia)dto.StatusFamilia.Value;
        await _familiaRepository.UpdateAsync(familia);
    }

    public async Task DeleteAsync(int id) => await _familiaRepository.DeleteAsync(id);

    public async Task LiberarParaProducaoAsync(int id)
    {
        var familia = await _familiaRepository.GetByIdAsync(id)
            ?? throw new InvalidOperationException("Família não encontrada.");
        if (familia.StatusFamilia is StatusFamilia.EmProducao or StatusFamilia.Produzida)
            throw new InvalidOperationException("Família já está liberada ou com produção finalizada.");

        var caixilhos = await _caixilhoRepository.GetListByFamiliaIdAsync(id);
        if (caixilhos.Count == 0 || !caixilhos.All(c => c.StatusProducao == StatusProducao.Medido))
            throw new InvalidOperationException("Todos os caixilhos devem estar medidos para liberar a produção.");

        familia.StatusFamilia = StatusFamilia.EmProducao;
        await _familiaRepository.UpdateAsync(familia);
    }

    public async Task FinalizarProducaoAsync(int id)
    {
        var familia = await _familiaRepository.GetByIdAsync(id)
            ?? throw new InvalidOperationException("Família não encontrada.");
        if (familia.StatusFamilia != StatusFamilia.EmProducao)
            throw new InvalidOperationException("Só é possível finalizar quando a família está liberada e em produção.");

        familia.StatusFamilia = StatusFamilia.Produzida;
        await _familiaRepository.UpdateAsync(familia);

        await _obraService.RecalcularProgressoAsync(familia.IdObra);
    }

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
