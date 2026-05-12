using API.SIGE.Model;

namespace API.SIGE.Interfaces.Repositories;

public interface IObraRepository
{
    Task<List<Obra>> GetAllAsync();
    Task<List<Obra>> GetAllFinalizadosAsync();
    Task<List<Obra>> GetAllNaoFinalizadosAsync();
    Task<List<Obra>> GetByStatusAsync(StatusObra status);
    Task<Obra?> GetById(int id);
    Task AddAsync(Obra obra);
    Task UpdateAsync(Obra obra);
    Task DeleteAsync(int id);
}
