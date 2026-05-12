using API.SIGE.DTOs;

namespace API.SIGE.Interfaces.Services;

public interface IAdminService
{
    Task<EmpresaResponseDto> AprovarSolicitacaoAsync(int idSolicitacao, AprovarSolicitacaoDto dto);
}
