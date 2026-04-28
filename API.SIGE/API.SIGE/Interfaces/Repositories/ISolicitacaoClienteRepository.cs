using API.SIGE.Model;

namespace API.SIGE.Interfaces.Repositories;

public interface ISolicitacaoClienteRepository
{
    Task<List<SolicitacaoCliente>> GetByClienteIdAsync(int clienteId);
    Task<List<SolicitacaoCliente>> GetByFamiliaIdAsync(int familiaId);
    Task<SolicitacaoCliente?> GetByIdAsync(int id);
    Task<SolicitacaoCliente?> GetByCaixilhoEClienteAsync(int caixilhoId, int clienteId);
    Task AddAsync(SolicitacaoCliente solicitacao);
    Task UpdateAsync(SolicitacaoCliente solicitacao);
    Task DeleteAsync(int id);
}