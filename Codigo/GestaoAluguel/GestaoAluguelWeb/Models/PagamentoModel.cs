using System.ComponentModel.DataAnnotations;

namespace GestaoAluguelWeb.Models
{
    public class PagamentoModel
    {
        [Display(Name = "Código")]
        [Key]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public int Id { get; set; }

        [Display(Name = "Valor")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public decimal Valor { get; set; }

        [Display(Name = "Data do pagamento")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public DateTime DataPagamento { get; set; }

        [Display(Name = "Tipo de pagamento")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string TipoPagamento { get; set; } = null!;
    }

    public enum TipoPagamento
    {
        Pix,
        Debito,
        Credito,
        Especime
    }
}
