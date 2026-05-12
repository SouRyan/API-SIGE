using API.SIGE.Model;

namespace API.SIGE.Interfaces.Repositories;

public interface IFamiliaCaixilhoRepository
{
    Task<List<FamiliaCaixilho>> GetAllAsync();
    Task<List<FamiliaCaixilho>> GetByObraIdAsync(int obraId);
    Task<int> CountByObraIdAsync(int obraId);
    Task AddAsync(FamiliaCaixilho familiaCaixilho);
    Task UpdateAsync(FamiliaCaixilho familiaCaixilho);
    Task DeleteAsync(int id);
    Task<FamiliaCaixilho?> GetByIdAsync(int id);
    Task<float> CalcularPesoTotalAsync(int familiaId);
    Task AtualizarPesoTotalAsync(int familiaId);
}
