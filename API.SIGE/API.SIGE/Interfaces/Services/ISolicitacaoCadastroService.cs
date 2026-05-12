using API.SIGE.DTOs;

namespace API.SIGE.Interfaces.Services;

public interface ISolicitacaoCadastroService
{
    Task<SolicitacaoCadastroResponseDto> SolicitarAsync(SolicitacaoCadastroCreateDto dto);
    Task<List<SolicitacaoCadastroResponseDto>> GetAllAsync();
    Task<List<SolicitacaoCadastroResponseDto>> GetPendentesAsync();
    Task<SolicitacaoCadastroResponseDto?> GetByIdAsync(int id);
    Task RecusarAsync(int id, string motivo);
}
