using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGE.API.Models
{
    [Table("Producao")]
    public class Producao
    {
        [Key]
        public int IdProducao { get; set; }


        [Required(ErrorMessage = "Campo Obrigatório")]
        public DateTime DataProducao { get; set; }

        //[Required]
        [StringLength(200)]
        public string? Descricao { get; set; }


        public EtapaProducao? Etapa { get; set; }

        //---------------------------------------- PARA QUE EU USARIA O USUARIO? PARA DEFINIR QUEM IRÁ FAZER E APENAS ELA?-----------------------
        //[ForeignKey("Usuario")]
        //public int IdUsuario { get; set; }
        //public virtual Usuario? Usuario { get; set; }


        [ForeignKey("FamiliaCaixilho")]
        public int FamiliaCaixilhoId { get; set; }
        public virtual FamiliaCaixilho? FamiliaCaixilho { get; set; }
    }

    public enum EtapaProducao
    {
        Pausado = 0,
        Corte = 1,
        Montagem = 2,
        Vedacao = 3,
        Acabamento = 4,
        Entrega = 5
    }



}
