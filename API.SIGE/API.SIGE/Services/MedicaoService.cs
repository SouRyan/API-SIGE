using API.SIGE.DTOs;
using API.SIGE.Interfaces.Repositories;
using API.SIGE.Interfaces.Services;
using API.SIGE.Models;

namespace API.SIGE.Services;

public class MedicaoService : IMedicaoService
{
    private readonly IMedicaoRepository _medicaoRepository;
    private readonly IFamiliaCaixilhoRepository _familiaRepository;
    private readonly IObraRepository _obraRepository;
    private readonly IObraService _obraService;
    private readonly INotificacaoService _notificacaoService;

    public MedicaoService(
        IMedicaoRepository medicaoRepository,
        IFamiliaCaixilhoRepository familiaRepository,
        IObraRepository obraRepository,
        IObraService obraService,
        INotificacaoService notificacaoService)
    {
        _medicaoRepository = medicaoRepository;
        _familiaRepository = familiaRepository;
        _obraRepository = obraRepository;
        _obraService = obraService;
        _notificacaoService = notificacaoService;
    }

    public async Task<MedicaoResponseDto?> GetByFamiliaIdAsync(int familiaId)
    {
        var m = await _medicaoRepository.GetByFamiliaIdAsync(familiaId);
        return m == null ? null : Map(m);
    }

    public async Task<MedicaoResponseDto?> GetByIdAsync(int id)
    {
        var m = await _medicaoRepository.GetByIdAsync(id);
        return m == null ? null : Map(m);
    }

    public async Task<MedicaoResponseDto> IniciarAsync(int familiaId, MedicaoIniciarDto dto)
    {
        var familia = await _familiaRepository.GetByIdAsync(familiaId)
            ?? throw new InvalidOperationException("Família não encontrada.");

        if (familia.StatusFamilia != StatusFamilia.Pendente)
            throw new InvalidOperationException("A família deve estar com status Pendente para iniciar a medição.");

        var existente = await _medicaoRepository.GetByFamiliaIdAsync(familiaId);
        if (existente != null && existente.Status != StatusAtividade.Concluida)
            throw new InvalidOperationException("Já existe uma medição em andamento para esta família.");

        var medicao = new Medicao
        {
            IdFamiliaCaixilho = familiaId,
            IdResponsavel = dto.IdResponsavel,
            Status = StatusAtividade.EmAndamento,
            DataInicio = DateTime.UtcNow,
            DataEstimadaConclusao = dto.DataEstimadaConclusao,
            Descricao = dto.Descricao
        };
        await _medicaoRepository.AddAsync(medicao);

        familia.StatusFamilia = StatusFamilia.EmMedicao;
        await _familiaRepository.UpdateAsync(familia);

        var obra = await _obraRepository.GetById(familia.IdObra);
        if (obra != null && obra.StatusObra == StatusObra.Verificada)
        {
            obra.StatusObra = StatusObra.EmMedicao;
            await _obraRepository.UpdateAsync(obra);
        }

        var loaded = await _medicaoRepository.GetByIdAsync(medicao.IdMedicao);
        return Map(loaded!);
    }

    public async Task<MedicaoResponseDto> PausarAsync(int familiaId, MedicaoPausarDto? dto)
    {
        var medicao = await _medicaoRepository.GetByFamiliaIdAsync(familiaId)
            ?? throw new InvalidOperationException("Medição não encontrada para esta família.");

        if (medicao.Status != StatusAtividade.EmAndamento)
            throw new InvalidOperationException("Somente medições em andamento podem ser pausadas.");

        medicao.Status = StatusAtividade.Pausada;
        if (dto?.Observacoes != null)
            medicao.Observacoes = dto.Observacoes;
        await _medicaoRepository.UpdateAsync(medicao);

        var reloaded = await _medicaoRepository.GetByIdAsync(medicao.IdMedicao);
        return Map(reloaded!);
    }

    public async Task<MedicaoResponseDto> FinalizarAsync(int familiaId, MedicaoFinalizarDto dto)
    {
        var familia = await _familiaRepository.GetByIdAsync(familiaId)
            ?? throw new InvalidOperationException("Família não encontrada.");

        var medicao = await _medicaoRepository.GetByFamiliaIdAsync(familiaId)
            ?? throw new InvalidOperationException("Medição não encontrada para esta família.");

        if (medicao.Status != StatusAtividade.EmAndamento && medicao.Status != StatusAtividade.Pausada)
            throw new InvalidOperationException("Medição não pode ser finalizada neste estado.");

        medicao.Status = StatusAtividade.Concluida;
        medicao.DataConclusao = DateTime.UtcNow;
        if (dto.Observacoes != null)
            medicao.Observacoes = dto.Observacoes;
        await _medicaoRepository.UpdateAsync(medicao);

        familia.StatusFamilia = StatusFamilia.Medida;
        await _familiaRepository.UpdateAsync(familia);

        await _obraService.RecalcularProgressoAsync(familia.IdObra);

        var obra = await _obraRepository.GetById(familia.IdObra);
        if (obra?.IdResponsavelProducao != null)
        {
            await _notificacaoService.CriarAsync(
                obra.IdResponsavelProducao.Value,
                "Família medida",
                $"A família {familia.DescricaoFamilia} foi medida.",
                TipoNotificacao.FamiliaMedida,
                obra.IdObra);
        }

        var final = await _medicaoRepository.GetByIdAsync(medicao.IdMedicao);
        return Map(final!);
    }

    private static MedicaoResponseDto Map(Medicao m) => new()
    {
        IdMedicao = m.IdMedicao,
        IdFamiliaCaixilho = m.IdFamiliaCaixilho,
        IdResponsavel = m.IdResponsavel,
        NomeResponsavel = m.Responsavel?.NomeUsuario,
        Status = m.Status,
        DataInicio = m.DataInicio,
        DataEstimadaConclusao = m.DataEstimadaConclusao,
        DataConclusao = m.DataConclusao,
        Descricao = m.Descricao,
        Observacoes = m.Observacoes
    };
}
