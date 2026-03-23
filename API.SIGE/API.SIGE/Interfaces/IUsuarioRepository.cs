using SIGE.API.Models;

namespace API.SIGE.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<List<Usuario>> GetAllAsync();

        Task<Usuario?> GetById(int id);
        Task<List<Usuario>> GetAllAtivosAsync();
        Task<List<Usuario>> GetAllInativosAsync();
        Task AddAsync(Usuario usuario);
        Task UpdateAsync(Usuario usuario);
        Task Delete(int id);
        Task InativarAsync(int id);
        Task ReativarAsync(int id);
        Task<Usuario?> ValidarLoginAsync(string email, string senha);
       
    }
}
