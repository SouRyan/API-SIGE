using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API.SIGE.Model

{
    [Table("Producao")]
    public class Producao
    {
        [Key]
        public int IdProducao { get; set; }

        [Required]
        public String NomeProducao { get; set; }
        
    }
}