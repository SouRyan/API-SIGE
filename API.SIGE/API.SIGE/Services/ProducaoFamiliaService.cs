using API.SIGE.DTOs;
using API.SIGE.Interfaces.Repositories;
using API.SIGE.Interfaces.Services;
using API.SIGE.Models;

namespace API.SIGE.Services;

public class ProducaoFamiliaService : IProducaoFamiliaService
{
    private readonly IProducaoFamiliaRepository _producaoRepository;
    private readonly IFamiliaCaixilhoRepository _familiaRepository;
    private readonly IObraRepository _obraRepository;
    private readonly IObraService _obraService;
    private readonly INotificacaoService _notificacaoService;
    private readonly IUsuarioRepository _usuarioRepository;

    public ProducaoFamiliaService(
        IProducaoFamiliaRepository producaoRepository,
        IFamiliaCaixilhoRepository familiaRepository,
        IObraRepository obraRepository,
        IObraService obraService,
        INotificacaoService notificacaoService,
        IUsuarioRepository usuarioRepository)
    {
        _producaoRepository = producaoRepository;
        _familiaRepository = familiaRepository;
        _obraRepository = obraRepository;
        _obraService = obraService;
        _notificacaoService = notificacaoService;
        _usuarioRepository = usuarioRepository;
    }

    public async Task<ProducaoFamiliaResponseDto?> GetByFamiliaIdAsync(int familiaId)
    {
        var p = await _producaoRepository.GetByFamiliaIdAsync(familiaId);
        return p == null ? null : Map(p);
    }

    public async Task<ProducaoFamiliaResponseDto?> GetByIdAsync(int id)
    {
        var p = await _producaoRepository.GetByIdAsync(id);
        return p == null ? null : Map(p);
    }

    public async Task<ProducaoFamiliaResponseDto> IniciarAsync(int familiaId, ProducaoFamiliaIniciarDto dto)
    {
        var familia = await _familiaRepository.GetByIdAsync(familiaId)
            ?? throw new InvalidOperationException("Família não encontrada.");

        if (familia.StatusFamilia != StatusFamilia.Medida)
            throw new InvalidOperationException("A família deve estar com status Medida para iniciar a produção.");

        var existente = await _producaoRepository.GetByFamiliaIdAsync(familiaId);
        if (existente != null && existente.Status != StatusAtividade.Concluida)
            throw new InvalidOperationException("Já existe uma produção em andamento para esta família.");

        var producao = new ProducaoFamilia
        {
            IdFamiliaCaixilho = familiaId,
            IdResponsavel = dto.IdResponsavel,
            Status = StatusAtividade.EmAndamento,
            DataInicio = DateTime.UtcNow,
            DataEstimadaConclusao = dto.DataEstimadaConclusao,
            Descricao = dto.Descricao
        };
        await _producaoRepository.AddAsync(producao);

        familia.StatusFamilia = StatusFamilia.EmProducao;
        await _familiaRepository.UpdateAsync(familia);

        var obra = await _obraRepository.GetById(familia.IdObra);
        if (obra != null && obra.StatusObra != StatusObra.Concluida)
        {
            obra.StatusObra = StatusObra.EmProducao;
            await _obraRepository.UpdateAsync(obra);
        }

        var loaded = await _producaoRepository.GetByIdAsync(producao.IdProducaoFamilia);
        return Map(loaded!);
    }

    public async Task<ProducaoFamiliaResponseDto> PausarAsync(int familiaId, ProducaoFamiliaPausarDto? dto)
    {
        var producao = await _producaoRepository.GetByFamiliaIdAsync(familiaId)
            ?? throw new InvalidOperationException("Produção não encontrada para esta família.");

        if (producao.Status != StatusAtividade.EmAndamento)
            throw new InvalidOperationException("Somente produções em andamento podem ser pausadas.");

        producao.Status = StatusAtividade.Pausada;
        if (dto?.Observacoes != null)
            producao.Observacoes = dto.Observacoes;
        await _producaoRepository.UpdateAsync(producao);

        var loaded = await _producaoRepository.GetByIdAsync(producao.IdProducaoFamilia);
        return Map(loaded!);
    }

    public async Task<ProducaoFamiliaResponseDto> FinalizarAsync(int familiaId, ProducaoFamiliaFinalizarDto dto)
    {
        var familia = await _familiaRepository.GetByIdAsync(familiaId)
            ?? throw new InvalidOperationException("Família não encontrada.");

        var producao = await _producaoRepository.GetByFamiliaIdAsync(familiaId)
            ?? throw new InvalidOperationException("Produção não encontrada para esta família.");

        if (producao.Status != StatusAtividade.EmAndamento && producao.Status != StatusAtividade.Pausada)
            throw new InvalidOperationException("Produção não pode ser finalizada neste estado.");

        producao.Status = StatusAtividade.Concluida;
        producao.DataConclusao = DateTime.UtcNow;
        if (dto.Observacoes != null)
            producao.Observacoes = dto.Observacoes;
        await _producaoRepository.UpdateAsync(producao);

        familia.StatusFamilia = StatusFamilia.Produzida;
        await _familiaRepository.UpdateAsync(familia);

        await _obraService.RecalcularProgressoAsync(familia.IdObra);

        var obra = await _obraRepository.GetById(familia.IdObra);
        if (obra != null && obra.PercentualMedicao >= 99.99f && obra.PercentualProducao >= 99.99f)
        {
            var gerentes = await _usuarioRepository.GetByCargoAsync(TipoCargo.Gerente);
            foreach (var g in gerentes)
            {
                await _notificacaoService.CriarAsync(
                    g.IdUsuario,
                    "Medição e produção concluídas",
                    $"A obra {obra.Nome} atingiu 100% de medição e produção.",
                    TipoNotificacao.FamiliaProduzida,
                    obra.IdObra);
            }
        }

        var loaded = await _producaoRepository.GetByIdAsync(producao.IdProducaoFamilia);
        return Map(loaded!);
    }

    private static ProducaoFamiliaResponseDto Map(ProducaoFamilia p) => new()
    {
        IdProducaoFamilia = p.IdProducaoFamilia,
        IdFamiliaCaixilho = p.IdFamiliaCaixilho,
        IdResponsavel = p.IdResponsavel,
        NomeResponsavel = p.Responsavel?.NomeUsuario,
        Status = p.Status,
        DataInicio = p.DataInicio,
        DataEstimadaConclusao = p.DataEstimadaConclusao,
        DataConclusao = p.DataConclusao,
        Descricao = p.Descricao,
        Observacoes = p.Observacoes
    };
}
