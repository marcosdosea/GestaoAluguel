using System.ComponentModel.DataAnnotations;

namespace GestaoAluguelWeb.Models
{
    public class LocacaoModel
    {
        [Display(Name = "Código")]
        [Key]
        [Required(ErrorMessage = "O campo é obrigatório.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo é obrigatório.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataInicio { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DataFim { get; set; }

        [Required(ErrorMessage = "O campo é obrigatório.")]
        [Range(0.01, float.MaxValue, ErrorMessage = "O valor do aluguel deve ser maior que zero.")]
        public float ValorAluguel { get; set; }

        [Required(ErrorMessage = "O campo é obrigatório.")]
        [Range(0, 2, ErrorMessage = "O status deve ser 0 (ativo), 1 (cancelado) ou 2 (finalizado).")]
        public sbyte Status { get; set; }

        [Display(Name = "Código do imóvel")]
        [Required(ErrorMessage = "O campo é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O Id do imóvel deve ser maior que zero.")]
        public int IdImovel { get; set; }

        [Display(Name = "Código do inquilino")]
        [Required(ErrorMessage = "O campo é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O Id do inquilino deve ser maior que zero.")]
        public int IdInquilino { get; set; }

        public string? Motivo { get; set; }

        public byte[]? Contrato { get; set; }

        
    }
}
