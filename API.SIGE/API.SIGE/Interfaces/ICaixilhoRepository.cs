using API.SIGE.Models;

namespace API.SIGE.Interfaces
{
    public interface ICaixilhoRepository
    {
        Task<List<Caixilho>> GetAllAsync();
         Task<Caixilho?> GetById(int id);
         Task AddAsync(Caixilho caixilho);
         Task UpdateAsync(Caixilho caixilho);
         Task DeleteAsync(int id);
    }
}
