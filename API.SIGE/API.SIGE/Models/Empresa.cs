using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.SIGE.Model;

[Table("Empresa")]
public class Empresa{

    [Key]
    public int IdEmpresa { get; set; }

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

    public bool Ativo { get; set; } = true;

}
