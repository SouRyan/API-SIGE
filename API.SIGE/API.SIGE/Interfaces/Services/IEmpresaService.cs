using API.SIGE.DTOs;

namespace API.SIGE.Interfaces.Services;

public interface IEmpresaService
{
    Task<List<EmpresaResponseDto>> GetAllAsync();
    Task<EmpresaResponseDto?> GetByIdAsync(int id);
    Task<EmpresaResponseDto> CreateAsync(EmpresaCreateDto dto);
    Task UpdateAsync(int id, EmpresaCreateDto dto);
    Task DeleteAsync(int id);
    Task InativarAsync(int id);
    Task AtivarAsync(int id);
}
