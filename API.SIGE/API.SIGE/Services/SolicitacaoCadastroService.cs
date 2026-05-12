using API.SIGE.DTOs;
using API.SIGE.Interfaces.Repositories;
using API.SIGE.Interfaces.Services;
using API.SIGE.Model;

namespace API.SIGE.Services;

public class SolicitacaoCadastroService : ISolicitacaoCadastroService
{
    private readonly ISolicitacaoCadastroRepository _repository;

    public SolicitacaoCadastroService(ISolicitacaoCadastroRepository repository)
    {
        _repository = repository;
    }

    public async Task<SolicitacaoCadastroResponseDto> SolicitarAsync(SolicitacaoCadastroCreateDto dto)
    {
        var existente = await _repository.GetByCnpjAsync(dto.Cnpj);
        if (existente != null)
            throw new InvalidOperationException("Já existe uma solicitação pendente para este CNPJ.");

        var solicitacao = new SolicitacaoCadastro
        {
            NomeEmpresa = dto.NomeEmpresa,
            Cnpj = dto.Cnpj,
            Cep = dto.Cep,
            Bairro = dto.Bairro,
            Cidade = dto.Cidade,
            Uf = dto.Uf,
            Logradouro = dto.Logradouro,
            NomeResponsavel = dto.NomeResponsavel,
            EmailResponsavel = dto.EmailResponsavel,
            TelefoneResponsavel = dto.TelefoneResponsavel,
            Observacao = dto.Observacao,
            Status = StatusSolicitacaoCadastro.Pendente,
            DataSolicitacao = DateTime.UtcNow
        };

        await _repository.AddAsync(solicitacao);
        return Map(solicitacao);
    }

    public async Task<List<SolicitacaoCadastroResponseDto>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();
        return list.Select(Map).ToList();
    }

    public async Task<List<SolicitacaoCadastroResponseDto>> GetPendentesAsync()
    {
        var list = await _repository.GetPendentesAsync();
        return list.Select(Map).ToList();
    }

    public async Task<SolicitacaoCadastroResponseDto?> GetByIdAsync(int id)
    {
        var s = await _repository.GetByIdAsync(id);
        return s == null ? null : Map(s);
    }

    public async Task RecusarAsync(int id, string motivo)
    {
        var solicitacao = await _repository.GetByIdAsync(id)
            ?? throw new InvalidOperationException("Solicitação não encontrada.");

        if (solicitacao.Status != StatusSolicitacaoCadastro.Pendente)
            throw new InvalidOperationException("Somente solicitações pendentes podem ser recusadas.");

        solicitacao.Status = StatusSolicitacaoCadastro.Recusada;
        solicitacao.MotivoRecusa = motivo;
        solicitacao.DataAnalise = DateTime.UtcNow;
        await _repository.UpdateAsync(solicitacao);
    }

    private static SolicitacaoCadastroResponseDto Map(SolicitacaoCadastro s) => new()
    {
        IdSolicitacaoCadastro = s.IdSolicitacaoCadastro,
        NomeEmpresa = s.NomeEmpresa,
        Cnpj = s.Cnpj,
        Cep = s.Cep,
        Bairro = s.Bairro,
        Cidade = s.Cidade,
        Uf = s.Uf,
        Logradouro = s.Logradouro,
        NomeResponsavel = s.NomeResponsavel,
        EmailResponsavel = s.EmailResponsavel,
        TelefoneResponsavel = s.TelefoneResponsavel,
        Status = s.Status,
        DataSolicitacao = s.DataSolicitacao,
        DataAnalise = s.DataAnalise,
        Observacao = s.Observacao,
        MotivoRecusa = s.MotivoRecusa
    };
}
