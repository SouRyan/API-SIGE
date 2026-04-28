using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API.SIGE.Model
{
    [Table("SolicitacaoCliente")]
    public class SolicitacaoCliente
    {
        [Key]
        public int IdSolicitacao { get; set; }

        [ForeignKey("Caixilho")]
        public int IdCaixilho { get; set; }
        [JsonIgnore]
        public virtual Caixilho? Caixilho { get; set; }

        [ForeignKey("Cliente")]
        public int IdCliente { get; set; }
        [JsonIgnore]
        public virtual Usuario? Cliente { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public DateTime DataNecessidadeEmObra { get; set; }

        [StringLength(300)]
        public string? ObservacaoCliente { get; set; }

        public PrioridadeCliente Prioridade { get; set; } = PrioridadeCliente.Normal;

        public DateTime DataSolicitacao { get; set; } = DateTime.UtcNow;
    }

    public enum PrioridadeCliente
    {
        Normal = 0,
        Importante = 1,
        Urgente = 2
    }
}