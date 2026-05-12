using API.SIGE.DTOs;

namespace API.SIGE.Interfaces.Services;

public interface IMedicaoService
{
    Task<MedicaoResponseDto?> GetByFamiliaIdAsync(int familiaId);
    Task<MedicaoResponseDto?> GetByIdAsync(int id);
    Task<MedicaoResponseDto> IniciarAsync(int familiaId, MedicaoIniciarDto dto);
    Task<MedicaoResponseDto> PausarAsync(int familiaId, MedicaoPausarDto? dto);
    Task<MedicaoResponseDto> FinalizarAsync(int familiaId, MedicaoFinalizarDto dto);
}
