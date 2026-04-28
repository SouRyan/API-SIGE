using API.SIGE.DTOs;
using API.SIGE.Interfaces.Repositories;
using API.SIGE.Interfaces.Services;
using API.SIGE.Model;

namespace API.SIGE.Services;

public class ObraService : IObraService
{
    private readonly IObraRepository _obraRepository;
    private readonly IFamiliaCaixilhoRepository _familiaRepository;
    private readonly INotificacaoService _notificacaoService;
    private readonly IUsuarioRepository _usuarioRepository;

    public ObraService(
        IObraRepository obraRepository,
        IFamiliaCaixilhoRepository familiaRepository,
        INotificacaoService notificacaoService,
        IUsuarioRepository usuarioRepository)
    {
        _obraRepository = obraRepository;
        _familiaRepository = familiaRepository;
        _notificacaoService = notificacaoService;
        _usuarioRepository = usuarioRepository;
    }

    public async Task<List<ObraResponseDto>> GetAllAsync(bool? finalizadas)
    {
        List<Obra> list = finalizadas switch
        {
            true => await _obraRepository.GetAllFinalizadosAsync(),
            false => await _obraRepository.GetAllNaoFinalizadosAsync(),
            _ => await _obraRepository.GetAllAsync()
        };
        return list.Select(Map).ToList();
    }

    public async Task<ObraResponseDto?> GetByIdAsync(int id)
    {
        var obra = await _obraRepository.GetById(id);
        return obra == null ? null : Map(obra);
    }

    public async Task<ObraResponseDto> CreateAsync(ObraCreateDto dto)
    {
        var obra = new Obra
        {
            Nome = dto.Nome,
            Construtora = dto.Construtora,
            Nro = dto.Nro,
            Logradouro = dto.Logradouro,
            Bairro = dto.Bairro,
            Cep = dto.Cep,
            Uf = dto.Uf,
            Cnpj = dto.Cnpj,
            DataInicio = dto.DataInicio,
            DataTermino = dto.DataTermino,
            PesoFinal = dto.PesoFinal,
            PesoProduzido = 0,
            PercentualConclusao = 0,
            Observacoes = dto.Observacoes,
            Finalizado = false,
            IdUsuario = dto.IdUsuario,
            StatusObra = StatusObra.Cadastrada,
            PercentualMedicao = 0,
            PercentualProducao = 0,
            IdCliente = dto.IdCliente
        };
        await _obraRepository.AddAsync(obra);

        // Notificar medidores e produtores sobre nova obra
        var medidores = await _usuarioRepository.GetByCargoAsync(TipoCargo.ResponsavelMedicao);
        var produtores = await _usuarioRepository.GetByCargoAsync(TipoCargo.ResponsavelProducao);
        var destinatarios = medidores.Concat(produtores).DistinctBy(u => u.IdUsuario);
        foreach (var u in destinatarios)
        {
            await _notificacaoService.CriarAsync(
                u.IdUsuario,
                "Nova obra cadastrada",
                $"A obra \"{obra.Nome}\" foi cadastrada no sistema. Fique atento para novas atividades.",
                TipoNotificacao.ObraCriada,
                obra.IdObra);
        }

        var created = await _obraRepository.GetById(obra.IdObra);
        return Map(created!);
    }

    public async Task UpdateAsync(int id, ObraUpdateDto dto)
    {
        var obra = await _obraRepository.GetById(id)
            ?? throw new InvalidOperationException("Obra não encontrada.");

        obra.Nome = dto.Nome;
        obra.Construtora = dto.Construtora;
        obra.Nro = dto.Nro;
        obra.Logradouro = dto.Logradouro;
        obra.Bairro = dto.Bairro;
        obra.Cep = dto.Cep;
        obra.Uf = dto.Uf;
        obra.Cnpj = dto.Cnpj;
        obra.DataInicio = dto.DataInicio;
        obra.DataTermino = dto.DataTermino;
        obra.PesoFinal = dto.PesoFinal;
        obra.Observacoes = dto.Observacoes;
        obra.Finalizado = dto.Finalizado;
        obra.ImagemObraPath = dto.ImagemObraPath;
        obra.IdUsuario = dto.IdUsuario;
        obra.IdCliente = dto.IdCliente;

        await _obraRepository.UpdateAsync(obra);
    }

    public async Task DeleteAsync(int id) => await _obraRepository.DeleteAsync(id);

    public Task<List<string>> ImportarObrasXmlAsync(List<IFormFile> arquivosXml)
    {
        var res = arquivosXml
            .Select(f => $"Arquivo recebido: {f.FileName} (importação XML não implementada).")
            .ToList();
        return Task.FromResult(res);
    }

    public async Task VerificarAsync(int id)
    {
        var obra = await _obraRepository.GetById(id)
            ?? throw new InvalidOperationException("Obra não encontrada.");

        if (obra.StatusObra != StatusObra.Cadastrada)
            throw new InvalidOperationException("Somente obras cadastradas podem ser verificadas.");

        obra.StatusObra = StatusObra.Verificada;
        await _obraRepository.UpdateAsync(obra);

        // Notificar responsáveis pela medição que há famílias disponíveis
        var medidores = await _usuarioRepository.GetByCargoAsync(TipoCargo.ResponsavelMedicao);
        foreach (var u in medidores)
        {
            await _notificacaoService.CriarAsync(
                u.IdUsuario,
                "Famílias disponíveis para medição",
                $"A obra \"{obra.Nome}\" foi verificada. As famílias estão disponíveis para medição e envio de fotos.",
                TipoNotificacao.FamiliaParaMedir,
                obra.IdObra);
        }
    }

    public async Task ConcluirAsync(int id)
    {
        await RecalcularProgressoAsync(id);
        var obra = await _obraRepository.GetById(id)
            ?? throw new InvalidOperationException("Obra não encontrada.");

        if (obra.PercentualMedicao < 99.99f || obra.PercentualProducao < 99.99f)
            throw new InvalidOperationException("É necessário 100% de medição e 100% de produção para concluir a obra.");

        obra.StatusObra = StatusObra.Concluida;
        obra.Finalizado = true;
        obra.DataConclusao = DateTime.UtcNow;
        await _obraRepository.UpdateAsync(obra);

        var gerentes = await _usuarioRepository.GetByCargoAsync(TipoCargo.Gerente);
        foreach (var g in gerentes)
        {
            await _notificacaoService.CriarAsync(
                g.IdUsuario,
                "Obra concluída",
                $"A obra {obra.Nome} foi concluída.",
                TipoNotificacao.ObraConcluida,
                obra.IdObra);
        }
    }

    public async Task RecalcularProgressoAsync(int id)
    {
        var obra = await _obraRepository.GetById(id);
        if (obra == null) return;

        var familias = await _familiaRepository.GetByObraIdAsync(id);
        var total = familias.Count;
        if (total == 0)
        {
            obra.PercentualMedicao = 0;
            obra.PercentualProducao = 0;
        }
        else
        {
            var medidas = familias.Count(f =>
                f.StatusFamilia is StatusFamilia.Medida or StatusFamilia.EmProducao or StatusFamilia.Produzida);
            var produzidas = familias.Count(f => f.StatusFamilia == StatusFamilia.Produzida);
            obra.PercentualMedicao = (float)(medidas * 100.0 / total);
            obra.PercentualProducao = (float)(produzidas * 100.0 / total);
        }

        await _obraRepository.UpdateAsync(obra);
    }

    private static ObraResponseDto Map(Obra o) => new()
    {
        IdObra = o.IdObra,
        Nome = o.Nome,
        Construtora = o.Construtora,
        Nro = o.Nro,
        Logradouro = o.Logradouro,
        Bairro = o.Bairro,
        Cep = o.Cep,
        Uf = o.Uf,
        Cnpj = o.Cnpj,
        DataInicio = o.DataInicio,
        DataTermino = o.DataTermino,
        PesoFinal = o.PesoFinal,
        PesoProduzido = o.PesoProduzido,
        PercentualConclusao = o.PercentualConclusao,
        DataConclusao = o.DataConclusao,
        Observacoes = o.Observacoes,
        Finalizado = o.Finalizado,
        ImagemObraPath = o.ImagemObraPath,
        IdUsuario = o.IdUsuario,
        NomeUsuario = o.Usuario?.NomeUsuario,
        IdCliente = o.IdCliente,
        StatusObra = o.StatusObra,
        PercentualMedicao = o.PercentualMedicao,
        PercentualProducao = o.PercentualProducao
    };
}
