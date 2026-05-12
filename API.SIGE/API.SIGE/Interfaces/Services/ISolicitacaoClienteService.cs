using API.SIGE.DTOs;

namespace API.SIGE.Interfaces.Services;

public interface ISolicitacaoClienteService
{
    Task<List<SolicitacaoClienteResponseDto>> GetByClienteIdAsync(int clienteId);
    Task<List<SolicitacaoClienteResponseDto>> GetByFamiliaIdAsync(int familiaId);
    Task<SolicitacaoClienteResponseDto> CreateAsync(int clienteId, SolicitacaoClienteCreateDto dto);
    Task UpdateAsync(int id, int clienteId, SolicitacaoClienteCreateDto dto);
    Task DeleteAsync(int id, int clienteId);
}
