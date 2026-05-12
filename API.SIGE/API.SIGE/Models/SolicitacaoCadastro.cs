using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.SIGE.Model;

[Table("SolicitacaoCadastro")]
public class SolicitacaoCadastro
{
    [Key]
    public int IdSolicitacaoCadastro { get; set; }

    [Required]
    [StringLength(100)]
    public string NomeEmpresa { get; set; } = string.Empty;

    [Required]
    [StringLength(18)]
    public string Cnpj { get; set; } = string.Empty;

    [Required]
    [StringLength(9)]
    public string Cep { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Bairro { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Cidade { get; set; } = string.Empty;

    [Required]
    [StringLength(2)]
    public string Uf { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Logradouro { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string NomeResponsavel { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string EmailResponsavel { get; set; } = string.Empty;

    [Required]
    [StringLength(14)]
    public string TelefoneResponsavel { get; set; } = string.Empty;

    public StatusSolicitacaoCadastro Status { get; set; } = StatusSolicitacaoCadastro.Pendente;

    public DateTime DataSolicitacao { get; set; } = DateTime.UtcNow;

    public DateTime? DataAnalise { get; set; }

    [StringLength(500)]
    public string? Observacao { get; set; }

    [StringLength(500)]
    public string? MotivoRecusa { get; set; }
}

public enum StatusSolicitacaoCadastro
{
    Pendente = 0,
    Aprovada = 1,
    Recusada = 2
}
