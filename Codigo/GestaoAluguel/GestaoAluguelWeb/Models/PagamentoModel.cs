using System.ComponentModel.DataAnnotations;
using Core;
using System.ComponentModel.DataAnnotations;
using Util;

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
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public int Id { get; set; }

        [Display(Name = "Valor *")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public float Valor { get; set; }

        [Display(Name = "Data de Pagamento *")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]  
        public DateTime DataPagamento { get; set; }


        [Display(Name = "Tipo de Pagamento *")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string TipoPagamento { get; set; } = null!;


        public virtual ICollection<Cobranca> IdCobrancas { get; set; } = new List<Cobranca>();
    }
}
