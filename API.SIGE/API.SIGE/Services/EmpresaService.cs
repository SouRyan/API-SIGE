using API.SIGE.DTOs;
using API.SIGE.Interfaces.Repositories;
using API.SIGE.Interfaces.Services;
using API.SIGE.Model;

namespace API.SIGE.Services;

public class EmpresaService : IEmpresaService
{
    private readonly IEmpresaRepository _empresaRepository;

    public EmpresaService(IEmpresaRepository empresaRepository)
    {
        _empresaRepository = empresaRepository;
    }

    public async Task<List<EmpresaResponseDto>> GetAllAsync()
    {
        var list = await _empresaRepository.GetAllAsync();
        return list.Select(Map).ToList();
    }

    public async Task<EmpresaResponseDto?> GetByIdAsync(int id)
    {
        var empresa = await _empresaRepository.GetById(id);
        return empresa == null ? null : Map(empresa);
    }

    public async Task<EmpresaResponseDto> CreateAsync(EmpresaCreateDto dto)
    {
        var empresa = new Empresa
        {
            NomeEmpresa = dto.NomeEmpresa,
            Cnpj = dto.Cnpj,
            Cep = dto.Cep,
            Bairro = dto.Bairro,
            EmailResponsavel = dto.EmailResponsavel,
            Telefone = dto.Telefone,
            Cidade = dto.Cidade,
            Uf = dto.Uf,
            Logradouro = dto.Logradouro,
            Ativo = true
        };

        await _empresaRepository.AddAsync(empresa);
        return Map(empresa);
    }

    public async Task UpdateAsync(int id, EmpresaCreateDto dto)
    {
        var empresa = await _empresaRepository.GetById(id)
            ?? throw new InvalidOperationException("Empresa não encontrada.");

        empresa.NomeEmpresa = dto.NomeEmpresa;
        empresa.Cnpj = dto.Cnpj;
        empresa.Cep = dto.Cep;
        empresa.Bairro = dto.Bairro;
        empresa.EmailResponsavel = dto.EmailResponsavel;
        empresa.Telefone = dto.Telefone;
        empresa.Cidade = dto.Cidade;
        empresa.Uf = dto.Uf;
        empresa.Logradouro = dto.Logradouro;

        await _empresaRepository.UpdateAsync(empresa);
    }

    public async Task DeleteAsync(int id)
    {
        await _empresaRepository.DeleteAsync(id);
    }

    public async Task InativarAsync(int id)
    {
        var empresa = await _empresaRepository.GetById(id)
            ?? throw new InvalidOperationException("Empresa não encontrada.");

        empresa.Ativo = false;
        await _empresaRepository.UpdateAsync(empresa);
    }

    public async Task AtivarAsync(int id)
    {
        var empresa = await _empresaRepository.GetById(id)
            ?? throw new InvalidOperationException("Empresa não encontrada.");

        empresa.Ativo = true;
        await _empresaRepository.UpdateAsync(empresa);
    }

    private static EmpresaResponseDto Map(Empresa e) => new()
    {
        IdEmpresa = e.IdEmpresa,
        NomeEmpresa = e.NomeEmpresa,
        Cnpj = e.Cnpj,
        Cep = e.Cep,
        Bairro = e.Bairro,
        EmailResponsavel = e.EmailResponsavel,
        Telefone = e.Telefone,
        Cidade = e.Cidade,
        Uf = e.Uf,
        Logradouro = e.Logradouro,
        Ativo = e.Ativo
    };
}
