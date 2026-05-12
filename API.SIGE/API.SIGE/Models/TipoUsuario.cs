using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API.SIGE.Model
{
    [Table("TipoUsuario")]
    public class TipoUsuario
    {
        [Key]
        public int IdTipoUsuario { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(100)]
        public string NomeTipoUsuario { get; set; }

        [Required]
        public int IdEmpresa { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(IdEmpresa))]
        public virtual Empresa? Empresa { get; set; }

        //[Key]
        //public TipoUsuarioEnum TipoUsuarioEnum { get; set; }

        //public List<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }

    //public enum TipoUsuarioEnum
    //{
    //    Administrador = 1,
    //    Gerente = 2,
    //    Funcionario = 3
    //}
}
