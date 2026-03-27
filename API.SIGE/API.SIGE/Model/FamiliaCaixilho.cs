using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using API.SIGE.Models;

namespace SIGE.API.Models
{
    [Table ("FamiliaCaixilho")]
    public class FamiliaCaixilho
    {
        [Key]
        public int IdFamiliaCaixilho { get; set; }
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Familia de Caixilhos deve ter entre 3 e 100 caracteres")]
        public string DescricaoFamilia{ get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public int PesoTotal { get; set; }

        [Required]
        public int IdObra { get; set; }

        public StatusFamilia StatusFamilia { get; set; } = StatusFamilia.Pendente;

        [JsonIgnore]
        public virtual Obra? Obra { get; set; }

        //Verificar
        // Propriedade calculada para exibir o peso total formatado
        [NotMapped]
        public string PesoTotalFormatado => $"{PesoTotal:F2} kg";


    }
}
