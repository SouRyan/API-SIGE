using API.SIGE.DTOs;
using API.SIGE.Interfaces.Repositories;
using API.SIGE.Interfaces.Services;
using API.SIGE.Models;
using SIGE.API.Models;

namespace API.SIGE.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IUsuarioCargoRepository _usuarioCargoRepository;
    private readonly ICargoRepository _cargoRepository;

    public UsuarioService(
        IUsuarioRepository usuarioRepository,
        IUsuarioCargoRepository usuarioCargoRepository,
        ICargoRepository cargoRepository)
    {
        _usuarioRepository = usuarioRepository;
        _usuarioCargoRepository = usuarioCargoRepository;
        _cargoRepository = cargoRepository;
    }

    public async Task<List<UsuarioResponseDto>> GetAllAsync()
    {
        var list = await _usuarioRepository.GetAllAsync();
        return list.Select(Map).ToList();
    }

    public async Task<List<UsuarioResponseDto>> GetAllAtivosAsync()
    {
        var list = await _usuarioRepository.GetAllAtivosAsync();
        return list.Select(Map).ToList();
    }

    public async Task<List<UsuarioResponseDto>> GetAllInativosAsync()
    {
        var list = await _usuarioRepository.GetAllInativosAsync();
        return list.Select(Map).ToList();
    }

    public async Task<List<UsuarioResponseDto>> GetByCargoAsync(TipoCargo tipoCargo)
    {
        var list = await _usuarioRepository.GetByCargoAsync(tipoCargo);
        return list.Select(Map).ToList();
    }

    public async Task<UsuarioResponseDto?> GetByIdAsync(int id)
    {
        var u = await _usuarioRepository.GetById(id);
        return u == null ? null : Map(u);
    }

    public async Task<UsuarioResponseDto> CreateAsync(UsuarioCreateDto dto)
    {
        var usuario = new Usuario
        {
            NomeUsuario = dto.NomeUsuario,
            Email = dto.Email,
            Senha = dto.Senha,
            Telefone = dto.Telefone,
            Ativo = true,
            IdTipoUsuario = dto.IdTipoUsuario
        };
        await _usuarioRepository.AddAsync(usuario);

        if (dto.IdCargos != null)
        {
            foreach (var idCargo in dto.IdCargos)
            {
                _ = await _cargoRepository.GetByIdAsync(idCargo)
                    ?? throw new InvalidOperationException($"Cargo {idCargo} não encontrado.");
                if (await _usuarioCargoRepository.ExistsAsync(usuario.IdUsuario, idCargo))
                    continue;
                await _usuarioCargoRepository.AddAsync(new UsuarioCargo
                {
                    IdUsuario = usuario.IdUsuario,
                    IdCargo = idCargo
                });
            }
        }

        var created = await _usuarioRepository.GetById(usuario.IdUsuario);
        return Map(created!);
    }

    public async Task UpdateAsync(int id, UsuarioUpdateDto dto)
    {
        var usuario = await _usuarioRepository.GetById(id)
            ?? throw new InvalidOperationException("Usuário não encontrado.");

        usuario.NomeUsuario = dto.NomeUsuario;
        usuario.Email = dto.Email;
        usuario.Telefone = dto.Telefone;
        usuario.IdTipoUsuario = dto.IdTipoUsuario;
        if (!string.IsNullOrWhiteSpace(dto.Senha))
            usuario.Senha = dto.Senha;

        await _usuarioRepository.UpdateAsync(usuario);
    }

    public async Task InativarAsync(int id) => await _usuarioRepository.InativarAsync(id);

    public async Task AtivarAsync(int id) => await _usuarioRepository.ReativarAsync(id);

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto)
    {
        var usuario = await _usuarioRepository.ValidarLoginAsync(dto.Email, dto.Senha);
        if (usuario == null)
        {
            return new LoginResponseDto
            {
                Success = false,
                Message = "E-mail ou senha inválidos."
            };
        }

        var cargos = usuario.UsuarioCargos?
            .Select(uc => uc.Cargo?.DescricaoCargo ?? uc.Cargo?.TipoCargo.ToString() ?? "")
            .Where(s => !string.IsNullOrEmpty(s))
            .ToList();

        return new LoginResponseDto
        {
            Success = true,
            IdUsuario = usuario.IdUsuario,
            NomeUsuario = usuario.NomeUsuario,
            Email = usuario.Email,
            TipoUsuario = usuario.IdTipoUsuario,
            Cargos = cargos
        };
    }

    public async Task AtribuirCargoAsync(int idUsuario, int idCargo)
    {
        _ = await _usuarioRepository.GetById(idUsuario)
            ?? throw new InvalidOperationException("Usuário não encontrado.");
        _ = await _cargoRepository.GetByIdAsync(idCargo)
            ?? throw new InvalidOperationException("Cargo não encontrado.");
        if (await _usuarioCargoRepository.ExistsAsync(idUsuario, idCargo))
            return;
        await _usuarioCargoRepository.AddAsync(new UsuarioCargo { IdUsuario = idUsuario, IdCargo = idCargo });
    }

    public async Task RemoverCargoAsync(int idUsuario, int idCargo)
    {
        var lista = await _usuarioCargoRepository.GetByUsuarioIdAsync(idUsuario);
        var uc = lista.FirstOrDefault(x => x.IdCargo == idCargo)
            ?? throw new InvalidOperationException("Usuário não possui este cargo.");
        await _usuarioCargoRepository.DeleteAsync(uc.IdUsuarioCargo);
    }

    private static UsuarioResponseDto Map(Usuario u) => new()
    {
        IdUsuario = u.IdUsuario,
        NomeUsuario = u.NomeUsuario,
        Email = u.Email,
        Telefone = u.Telefone,
        Ativo = u.Ativo,
        IdTipoUsuario = u.IdTipoUsuario,
        NomeTipoUsuario = u.TipoUsuario?.NomeTipoUsuario,
        Cargos = u.UsuarioCargos?
            .Where(uc => uc.Cargo != null)
            .Select(uc => new CargoResponseDto
            {
                IdCargo = uc.Cargo!.IdCargo,
                TipoCargo = uc.Cargo.TipoCargo,
                DescricaoCargo = uc.Cargo.DescricaoCargo
            }).ToList() ?? new List<CargoResponseDto>()
    };
}
