using System;
using System.ComponentModel.DataAnnotations;

namespace GestaoAluguelWeb.Models
{
    public class CobrancaModel
    {
        [Display(Name = "ID")]
        [Key]
        [Required(ErrorMessage = "Id da cobrança é obrigatório")]
        public int Id { get; set; }

        [Display(Name = "Valor")]
        [Required(ErrorMessage = "Valor da cobrança é obrigatório")]
        public float Valor { get; set; }
        [Display(Name = "Data da Cobrança")]

        public DateTime DataCobranca { get; set; } = DateTime.Now;
        [Display(Name = "Descrição")]
        [StringLength(45, ErrorMessage = "A descrição deve ter no máximo 45 caracteres")]

        public string Descricao { get; set; }

        [Display(Name = "Tipo de Cobrança")]
        [Required(ErrorMessage = "Tipo de cobrança é obrigatório")]
        public  string TipoCobranca { get; set; }

        [Display(Name = "ID da Locação")]
        [Required(ErrorMessage = "Id da locação é obrigatório")]
        public int IdLocacao { get; set; }

    }
}
