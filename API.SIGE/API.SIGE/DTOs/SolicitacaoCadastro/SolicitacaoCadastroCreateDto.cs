using System.ComponentModel.DataAnnotations;

namespace API.SIGE.DTOs;

public class SolicitacaoCadastroCreateDto
{
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
    [EmailAddress]
    public string EmailResponsavel { get; set; } = string.Empty;

    [Required]
    [StringLength(14)]
    public string TelefoneResponsavel { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Observacao { get; set; }
}
