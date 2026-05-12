namespace API.SIGE.DTOs;

public class EmpresaResponseDto
{
    public int IdEmpresa { get; set; }
    public string NomeEmpresa { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public string Cep { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string EmailResponsavel { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Uf { get; set; } = string.Empty;
    public string Logradouro { get; set; } = string.Empty;
    public bool Ativo { get; set; }
}
