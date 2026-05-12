using API.SIGE.DTOs;
using API.SIGE.Model;

namespace API.SIGE.Interfaces.Services;

public interface IUsuarioService
{
    Task<List<UsuarioResponseDto>> GetAllAsync();
    Task<List<UsuarioResponseDto>> GetAllAtivosAsync();
    Task<List<UsuarioResponseDto>> GetAllInativosAsync();
    Task<List<UsuarioResponseDto>> GetByCargoAsync(TipoCargo tipoCargo);
    Task<UsuarioResponseDto?> GetByIdAsync(int id);
    Task<UsuarioResponseDto> CreateAsync(UsuarioCreateDto dto);
    Task UpdateAsync(int id, UsuarioUpdateDto dto);
    Task InativarAsync(int id);
    Task AtivarAsync(int id);
    Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);
    Task AtribuirCargoAsync(int idUsuario, int idCargo);
    Task RemoverCargoAsync(int idUsuario);
}
