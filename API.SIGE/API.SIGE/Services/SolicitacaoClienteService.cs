using API.SIGE.DTOs;
using API.SIGE.Interfaces;
using API.SIGE.Interfaces.Repositories;
using API.SIGE.Interfaces.Services;
using API.SIGE.Model;

namespace API.SIGE.Services;

public class SolicitacaoClienteService : ISolicitacaoClienteService
{
    private readonly ISolicitacaoClienteRepository _repository;
    private readonly ITenantProvider _tenantProvider;

    public SolicitacaoClienteService(ISolicitacaoClienteRepository repository, ITenantProvider tenantProvider)
    {
        _repository = repository;
        _tenantProvider = tenantProvider;
    }

    public async Task<List<SolicitacaoClienteResponseDto>> GetByClienteIdAsync(int clienteId)
    {
        var lista = await _repository.GetByClienteIdAsync(clienteId);
        return lista.Select(Map).ToList();
    }

    public async Task<List<SolicitacaoClienteResponseDto>> GetByFamiliaIdAsync(int familiaId)
    {
        var lista = await _repository.GetByFamiliaIdAsync(familiaId);
        return lista.Select(Map).ToList();
    }

    public async Task<SolicitacaoClienteResponseDto> CreateAsync(int clienteId, SolicitacaoClienteCreateDto dto)
    {
        var existente = await _repository.GetByCaixilhoEClienteAsync(dto.IdCaixilho, clienteId);
        if (existente != null)
            throw new InvalidOperationException("Já existe uma solicitação para este caixilho.");

        var solicitacao = new SolicitacaoCliente
        {
            IdCaixilho = dto.IdCaixilho,
            IdCliente = clienteId,
            DataNecessidadeEmObra = dto.DataNecessidadeEmObra.ToUniversalTime(),
            ObservacaoCliente = dto.ObservacaoCliente,
            Prioridade = (PrioridadeCliente)dto.Prioridade,
            DataSolicitacao = DateTime.UtcNow,
            IdEmpresa = _tenantProvider.GetTenantId()
        };

        await _repository.AddAsync(solicitacao);

        var criada = await _repository.GetByIdAsync(solicitacao.IdSolicitacao);
        return Map(criada!);
    }

    public async Task UpdateAsync(int id, int clienteId, SolicitacaoClienteCreateDto dto)
    {
        var solicitacao = await _repository.GetByIdAsync(id)
            ?? throw new InvalidOperationException("Solicitação não encontrada.");

        if (solicitacao.IdCliente != clienteId)
            throw new InvalidOperationException("Sem permissão para editar esta solicitação.");

        solicitacao.DataNecessidadeEmObra = dto.DataNecessidadeEmObra.ToUniversalTime();
        solicitacao.ObservacaoCliente = dto.ObservacaoCliente;
        solicitacao.Prioridade = (PrioridadeCliente)dto.Prioridade;

        await _repository.UpdateAsync(solicitacao);
    }

    public async Task DeleteAsync(int id, int clienteId)
    {
        var solicitacao = await _repository.GetByIdAsync(id)
            ?? throw new InvalidOperationException("Solicitação não encontrada.");

        if (solicitacao.IdCliente != clienteId)
            throw new InvalidOperationException("Sem permissão para excluir esta solicitação.");

        await _repository.DeleteAsync(id);
    }

    private static SolicitacaoClienteResponseDto Map(SolicitacaoCliente s) => new()
    {
        IdSolicitacao = s.IdSolicitacao,
        IdCaixilho = s.IdCaixilho,
        NomeCaixilho = s.Caixilho?.NomeCaixilho,
        IdCliente = s.IdCliente,
        NomeCliente = s.Cliente?.NomeUsuario,
        DataNecessidadeEmObra = s.DataNecessidadeEmObra,
        ObservacaoCliente = s.ObservacaoCliente,
        Prioridade = (int)s.Prioridade,
        DataSolicitacao = s.DataSolicitacao,
        NomeObra = s.Caixilho?.Obra?.Nome,
        IdObra = s.Caixilho?.Obra?.IdObra ?? 0
    };
}
