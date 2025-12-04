using System.ComponentModel.DataAnnotations;

namespace GestaoAluguelWeb.Models
{
    public class LocacaoModel
    {
        [Display(Name = "Código")]
        [Key]
        // Id geralmente não precisa de Required se for auto-incremento, mas ok manter
        public int Id { get; set; }

        [Required(ErrorMessage = "A data de início é obrigatória.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataInicio { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DataFim { get; set; }

        [Required(ErrorMessage = "O valor é obrigatório.")]
        [Range(0.01, float.MaxValue, ErrorMessage = "O valor do aluguel deve ser maior que zero.")]
        public float ValorAluguel { get; set; }

        [Required(ErrorMessage = "O status é obrigatório.")]
        // Dica: sbyte pode dar problema em alguns bancos, int é mais seguro, mas se já funciona, mantenha.
        public sbyte Status { get; set; } = 1; // Valor padrão definido aqui

        [Display(Name = "Imóvel")]
        [Required(ErrorMessage = "Selecione um imóvel.")]
        public int IdImovel { get; set; }

        // --- MUDANÇAS AQUI ---

        // 1. Removi o [Range(1...)] e o [Required] rígido.
        // Se for uma pessoa nova, isso aqui chega como 0 e nós preenchemos no Controller.
        public int IdInquilino { get; set; }

        // 2. Adicionei o objeto completo para capturar os dados na View (Create)
        public PessoaModel Inquilino { get; set; } = new PessoaModel();

        // ---------------------

        public string? Motivo { get; set; }

        public byte[]? Contrato { get; set; }
    }
}