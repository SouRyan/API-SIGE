using API.SIGE.Model;

namespace API.SIGE.Interfaces.Repositories;

public interface ISolicitacaoCadastroRepository
{
    Task<List<SolicitacaoCadastro>> GetAllAsync();
    Task<List<SolicitacaoCadastro>> GetPendentesAsync();
    Task<SolicitacaoCadastro?> GetByIdAsync(int id);
    Task<SolicitacaoCadastro?> GetByCnpjAsync(string cnpj);
    Task AddAsync(SolicitacaoCadastro solicitacao);
    Task UpdateAsync(SolicitacaoCadastro solicitacao);
}
