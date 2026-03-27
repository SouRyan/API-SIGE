using API.SIGE.Models;

namespace API.SIGE.Interfaces.Repositories;

public interface IProducaoFamiliaRepository
{
    Task<List<ProducaoFamilia>> GetAllAsync();
    Task<ProducaoFamilia?> GetByIdAsync(int id);
    Task<ProducaoFamilia?> GetByFamiliaIdAsync(int familiaId);
    Task AddAsync(ProducaoFamilia producao);
    Task UpdateAsync(ProducaoFamilia producao);
    Task DeleteAsync(int id);
}
