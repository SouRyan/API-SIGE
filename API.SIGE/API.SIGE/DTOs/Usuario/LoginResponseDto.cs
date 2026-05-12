namespace API.SIGE.DTOs
{
    public class LoginResponseDto
    {
        public bool Success { get; set; }
        public int? IdUsuario { get; set; }
        public string? NomeUsuario { get; set; }
        public string? Email { get; set; }
        public int? TipoUsuario { get; set; }
        /// <summary>Nome do TipoUsuario (Administrador, Operador, Vizualizador, ...). Usado para mapear roles no website.</summary>
        public string? NomeTipoUsuario { get; set; }
        public string? Message { get; set; }
        /// <summary>Descrição do cargo do usuário, se houver.</summary>
        public string? Cargo { get; set; }

        public string? Token { get; set; }

        public int? IdEmpresa { get; set; }
        public string? NomeEmpresa { get; set; }
    }
}
