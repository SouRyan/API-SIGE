using API.SIGE.DTOs;
using API.SIGE.Interfaces;
using API.SIGE.Interfaces.Repositories;
using API.SIGE.Interfaces.Services;
using API.SIGE.Model;

namespace API.SIGE.Services;

public class TipoUsuarioService : ITipoUsuarioService
{
    private readonly ITipoUsuarioRepository _tipoUsuarioRepository;
    private readonly ITenantProvider _tenantProvider;

    public TipoUsuarioService(ITipoUsuarioRepository tipoUsuarioRepository, ITenantProvider tenantProvider)
    {
        _tipoUsuarioRepository = tipoUsuarioRepository;
        _tenantProvider = tenantProvider;
    }

    public async Task<List<TipoUsuarioResponseDto>> GetAllAsync()
    {
        var list = await _tipoUsuarioRepository.GetAllAsync();
        return list.Select(Map).ToList();
    }

    public async Task<TipoUsuarioResponseDto?> GetByIdAsync(int id)
    {
        var t = await _tipoUsuarioRepository.GetById(id);
        return t == null ? null : Map(t);
    }

    public async Task<TipoUsuarioResponseDto> CreateAsync(TipoUsuarioCreateDto dto)
    {
        var entity = new TipoUsuario
        {
            NomeTipoUsuario = dto.NomeTipoUsuario,
            IdEmpresa = _tenantProvider.GetTenantId()
        };
        await _tipoUsuarioRepository.AddAsync(entity);
        var created = await _tipoUsuarioRepository.GetById(entity.IdTipoUsuario);
        return Map(created!);
    }

    public async Task UpdateAsync(int id, TipoUsuarioCreateDto dto)
    {
        var entity = await _tipoUsuarioRepository.GetById(id)
            ?? throw new InvalidOperationException("Tipo de usuário não encontrado.");
        entity.NomeTipoUsuario = dto.NomeTipoUsuario;
        await _tipoUsuarioRepository.UpdateAsync(entity);
    }

    public async Task DeleteAsync(int id) => await _tipoUsuarioRepository.DeleteAsync(id);

    private static TipoUsuarioResponseDto Map(TipoUsuario t) => new()
    {
        IdTipoUsuario = t.IdTipoUsuario,
        NomeTipoUsuario = t.NomeTipoUsuario
    };
}
