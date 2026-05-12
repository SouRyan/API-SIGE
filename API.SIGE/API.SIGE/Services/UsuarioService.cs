using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.SIGE.DTOs;
using API.SIGE.Interfaces;
using API.SIGE.Interfaces.Repositories;
using API.SIGE.Interfaces.Services;
using API.SIGE.Model;
using Microsoft.IdentityModel.Tokens;

namespace API.SIGE.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ICargoRepository _cargoRepository;
    private readonly IConfiguration _configuration;
    private readonly ITenantProvider _tenantProvider;

    public UsuarioService(
        IUsuarioRepository usuarioRepository,
        ICargoRepository cargoRepository,
        IConfiguration configuration,
        ITenantProvider tenantProvider)
    {
        _usuarioRepository = usuarioRepository;
        _cargoRepository = cargoRepository;
        _configuration = configuration;
        _tenantProvider = tenantProvider;
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
        if (dto.IdCargo.HasValue)
        {
            _ = await _cargoRepository.GetByIdAsync(dto.IdCargo.Value)
                ?? throw new InvalidOperationException($"Cargo {dto.IdCargo} não encontrado.");
        }

        var tenantId = _tenantProvider.GetTenantId();
        if (tenantId == 0)
            throw new InvalidOperationException("Tenant não identificado. Faça login novamente.");

        var usuario = new Usuario
        {
            NomeUsuario = dto.NomeUsuario,
            Email = dto.Email,
            Senha = dto.Senha,
            Telefone = dto.Telefone,
            Ativo = true,
            IdTipoUsuario = dto.IdTipoUsuario,
            IdCargo = dto.IdCargo,
            IdEmpresa = tenantId
        };
        await _usuarioRepository.AddAsync(usuario);

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

        if (!usuario.Ativo)
        {
            return new LoginResponseDto
            {
                Success = false,
                Message = "Usuário inativo. Entre em contato com o administrador."
            };
        }

        var empresa = usuario.Empresa;
        if (empresa != null && !empresa.Ativo)
        {
            return new LoginResponseDto
            {
                Success = false,
                Message = "Empresa inativa. Entre em contato com o suporte."
            };
        }

        var token = GerarToken(usuario);

        return new LoginResponseDto
        {
            Success = true,
            IdUsuario = usuario.IdUsuario,
            NomeUsuario = usuario.NomeUsuario,
            Email = usuario.Email,
            TipoUsuario = usuario.IdTipoUsuario,
            NomeTipoUsuario = usuario.TipoUsuario?.NomeTipoUsuario,
            Cargo = usuario.Cargo?.DescricaoCargo,
            Token = token,
            IdEmpresa = usuario.IdEmpresa,
            NomeEmpresa = empresa?.NomeEmpresa
        };
    }

    private string GerarToken(Usuario usuario)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
            new(ClaimTypes.Name, usuario.NomeUsuario),
            new(ClaimTypes.Email, usuario.Email),
            new("tipoUsuario", usuario.IdTipoUsuario.ToString()),
            new("empresaId", usuario.IdEmpresa.ToString())
        };

        if (usuario.Cargo != null)
            claims.Add(new Claim(ClaimTypes.Role, usuario.Cargo.DescricaoCargo));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task AtribuirCargoAsync(int idUsuario, int idCargo)
    {
        var usuario = await _usuarioRepository.GetById(idUsuario)
            ?? throw new InvalidOperationException("Usuário não encontrado.");
        _ = await _cargoRepository.GetByIdAsync(idCargo)
            ?? throw new InvalidOperationException("Cargo não encontrado.");
        usuario.IdCargo = idCargo;
        await _usuarioRepository.UpdateAsync(usuario);
    }

    public async Task RemoverCargoAsync(int idUsuario)
    {
        var usuario = await _usuarioRepository.GetById(idUsuario)
            ?? throw new InvalidOperationException("Usuário não encontrado.");
        usuario.IdCargo = null;
        await _usuarioRepository.UpdateAsync(usuario);
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
        Cargo = u.Cargo == null
            ? null
            : new CargoResponseDto
            {
                IdCargo = u.Cargo.IdCargo,
                TipoCargo = u.Cargo.TipoCargo,
                DescricaoCargo = u.Cargo.DescricaoCargo
            }
    };
}
