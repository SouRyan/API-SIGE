using API.SIGE.DTOs;
using API.SIGE.Interfaces.Repositories;
using API.SIGE.Interfaces.Services;
using API.SIGE.Model;

namespace API.SIGE.Services;

public class CaixilhoService : ICaixilhoService
{
    private readonly ICaixilhoRepository _caixilhoRepository;
    private readonly IFamiliaCaixilhoRepository _familiaRepository;

    public CaixilhoService(ICaixilhoRepository caixilhoRepository, IFamiliaCaixilhoRepository familiaRepository)
    {
        _caixilhoRepository = caixilhoRepository;
        _familiaRepository = familiaRepository;
    }

    public async Task<List<CaixilhoResponseDto>> GetAllAsync()
    {
        var list = await _caixilhoRepository.GetAllAsync();
        return list.Select(Map).ToList();
    }

    public async Task<CaixilhoResponseDto?> GetByIdAsync(int id)
    {
        var c = await _caixilhoRepository.GetById(id);
        return c == null ? null : Map(c);
    }

    public async Task<CaixilhoResponseDto> CreateAsync(CaixilhoCreateDto dto)
    {
        var familia = await _familiaRepository.GetByIdAsync(dto.IdFamiliaCaixilho)
            ?? throw new InvalidOperationException("Família não encontrada.");

        if (familia.IdObra != dto.ObraId)
            throw new InvalidOperationException("O caixilho deve pertencer à mesma obra da família.");

        var caixilho = new Caixilho
        {
            NomeCaixilho = dto.NomeCaixilho,
            Largura = dto.Largura,
            Altura = dto.Altura,
            Quantidade = dto.Quantidade,
            PesoUnitario = dto.PesoUnitario,
            Observacoes = dto.Observacoes,
            DescricaoCaixilho = dto.DescricaoCaixilho,
            StatusProducao = dto.StatusProducao,
            ObraId = dto.ObraId,
            IdFamiliaCaixilho = dto.IdFamiliaCaixilho
        };
        await _caixilhoRepository.AddAsync(caixilho);
        var created = await _caixilhoRepository.GetById(caixilho.IdCaixilho);
        return Map(created!);
    }

    public async Task UpdateAsync(int id, CaixilhoUpdateDto dto)
    {
        var existente = await _caixilhoRepository.GetById(id)
            ?? throw new InvalidOperationException("Caixilho não encontrado.");

        var familia = await _familiaRepository.GetByIdAsync(dto.IdFamiliaCaixilho)
            ?? throw new InvalidOperationException("Família não encontrada.");
        if (familia.IdObra != dto.ObraId)
            throw new InvalidOperationException("O caixilho deve pertencer à mesma obra da família.");

        existente.NomeCaixilho = dto.NomeCaixilho;
        existente.Largura = dto.Largura;
        existente.Altura = dto.Altura;
        existente.Quantidade = dto.Quantidade;
        existente.PesoUnitario = dto.PesoUnitario;
        existente.Observacoes = dto.Observacoes;
        existente.DescricaoCaixilho = dto.DescricaoCaixilho;
        existente.StatusProducao = dto.StatusProducao;
        existente.ObraId = dto.ObraId;
        existente.IdFamiliaCaixilho = dto.IdFamiliaCaixilho;
        existente.Liberado = dto.Liberado;
        existente.DataLiberacao = dto.DataLiberacao;

        await _caixilhoRepository.UpdateAsync(existente);
    }

    public async Task DeleteAsync(int id) => await _caixilhoRepository.DeleteAsync(id);

    public async Task LiberarAsync(int id)
    {
        var c = await _caixilhoRepository.GetById(id)
            ?? throw new InvalidOperationException("Caixilho não encontrado.");
        c.Liberado = true;
        c.DataLiberacao = DateTime.UtcNow;
        await _caixilhoRepository.UpdateAsync(c);
    }

    private static CaixilhoResponseDto Map(Caixilho c) => new()
    {
        IdCaixilho = c.IdCaixilho,
        NomeCaixilho = c.NomeCaixilho,
        Largura = c.Largura,
        Altura = c.Altura,
        Quantidade = c.Quantidade,
        PesoUnitario = c.PesoUnitario,
        Liberado = c.Liberado,
        DataLiberacao = c.DataLiberacao,
        Observacoes = c.Observacoes,
        DescricaoCaixilho = c.DescricaoCaixilho,
        StatusProducao = c.StatusProducao,
        ObraId = c.ObraId,
        NomeObra = c.Obra?.Nome,
        IdFamiliaCaixilho = c.IdFamiliaCaixilho,
        DescricaoFamilia = c.FamiliaCaixilho?.DescricaoFamilia
    };
}
