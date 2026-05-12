using API.SIGE.Data;
using API.SIGE.DTOs;
using API.SIGE.Interfaces.Repositories;
using API.SIGE.Interfaces.Services;
using API.SIGE.Model;
using Microsoft.EntityFrameworkCore;

namespace API.SIGE.Services;

public class AdminService : IAdminService
{
    private readonly AppDbData _context;
    private readonly ISolicitacaoCadastroRepository _solicitacaoRepository;

    public AdminService(AppDbData context, ISolicitacaoCadastroRepository solicitacaoRepository)
    {
        _context = context;
        _solicitacaoRepository = solicitacaoRepository;
    }

    public async Task<EmpresaResponseDto> AprovarSolicitacaoAsync(int idSolicitacao, AprovarSolicitacaoDto dto)
    {
        var solicitacao = await _solicitacaoRepository.GetByIdAsync(idSolicitacao)
            ?? throw new InvalidOperationException("Solicitação não encontrada.");

        if (solicitacao.Status != StatusSolicitacaoCadastro.Pendente)
            throw new InvalidOperationException("Somente solicitações pendentes podem ser aprovadas.");

        var cnpjExistente = await _context.Empresas
            .IgnoreQueryFilters()
            .AnyAsync(e => e.Cnpj == solicitacao.Cnpj);
        if (cnpjExistente)
            throw new InvalidOperationException("Já existe uma empresa cadastrada com este CNPJ.");

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var empresa = new Empresa
            {
                NomeEmpresa = solicitacao.NomeEmpresa,
                Cnpj = solicitacao.Cnpj,
                Cep = solicitacao.Cep,
                Bairro = solicitacao.Bairro,
                Cidade = solicitacao.Cidade,
                Uf = solicitacao.Uf,
                Logradouro = solicitacao.Logradouro,
                EmailResponsavel = solicitacao.EmailResponsavel,
                Telefone = solicitacao.TelefoneResponsavel,
                Ativo = true
            };
            await _context.Empresas.AddAsync(empresa);
            await _context.SaveChangesAsync();

            var tipoUsuario = new TipoUsuario
            {
                NomeTipoUsuario = "Administrador",
                IdEmpresa = empresa.IdEmpresa
            };
            await _context.TiposUsuario.AddAsync(tipoUsuario);
            await _context.SaveChangesAsync();

            var usuario = new Usuario
            {
                NomeUsuario = solicitacao.NomeResponsavel,
                Email = solicitacao.EmailResponsavel,
                Senha = dto.SenhaUsuarioAdmin,
                Telefone = solicitacao.TelefoneResponsavel,
                Ativo = true,
                IdTipoUsuario = dto.IdTipoUsuarioAdmin > 0 ? dto.IdTipoUsuarioAdmin : tipoUsuario.IdTipoUsuario,
                IdEmpresa = empresa.IdEmpresa
            };
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();

            solicitacao.Status = StatusSolicitacaoCadastro.Aprovada;
            solicitacao.DataAnalise = DateTime.UtcNow;
            await _solicitacaoRepository.UpdateAsync(solicitacao);

            await transaction.CommitAsync();

            return new EmpresaResponseDto
            {
                IdEmpresa = empresa.IdEmpresa,
                NomeEmpresa = empresa.NomeEmpresa,
                Cnpj = empresa.Cnpj,
                Cep = empresa.Cep,
                Bairro = empresa.Bairro,
                EmailResponsavel = empresa.EmailResponsavel,
                Telefone = empresa.Telefone,
                Cidade = empresa.Cidade,
                Uf = empresa.Uf,
                Logradouro = empresa.Logradouro,
                Ativo = empresa.Ativo
            };
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
