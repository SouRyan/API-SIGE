using System.ComponentModel.DataAnnotations;

namespace API.SIGE.DTOs;

public class EmpresaCreateDto
{
    [Required]
    [StringLength(100)]
    public string NomeEmpresa { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Cnpj { get; set; } = string.Empty;

    [Required]
    [StringLength(9)]
    public string Cep { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Bairro { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string EmailResponsavel { get; set; } = string.Empty;

    [Required]
    [StringLength(14)]
    public string Telefone { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Cidade { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Uf { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Logradouro { get; set; } = string.Empty;
}
