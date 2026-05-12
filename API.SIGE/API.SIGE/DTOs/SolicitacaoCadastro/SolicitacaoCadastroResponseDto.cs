using API.SIGE.Model;

namespace API.SIGE.DTOs;

public class SolicitacaoCadastroResponseDto
{
    public int IdSolicitacaoCadastro { get; set; }
    public string NomeEmpresa { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public string Cep { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Uf { get; set; } = string.Empty;
    public string Logradouro { get; set; } = string.Empty;
    public string NomeResponsavel { get; set; } = string.Empty;
    public string EmailResponsavel { get; set; } = string.Empty;
    public string TelefoneResponsavel { get; set; } = string.Empty;
    public StatusSolicitacaoCadastro Status { get; set; }
    public DateTime DataSolicitacao { get; set; }
    public DateTime? DataAnalise { get; set; }
    public string? Observacao { get; set; }
    public string? MotivoRecusa { get; set; }
}
