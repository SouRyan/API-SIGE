using API.SIGE.Models;

namespace API.SIGE.Interfaces.Repositories;

public interface IUsuarioCargoRepository
{
    Task<List<UsuarioCargo>> GetByUsuarioIdAsync(int idUsuario);
    Task<UsuarioCargo?> GetByIdAsync(int idUsuarioCargo);
    Task AddAsync(UsuarioCargo usuarioCargo);
    Task DeleteAsync(int idUsuarioCargo);
    Task<bool> ExistsAsync(int idUsuario, int idCargo);
}
