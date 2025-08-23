using System.ComponentModel.DataAnnotations;

namespace GestaoAluguelWeb.Models
{
    public class ImovelModel
    {
        [Display(Name = "Código")]
        [Key]
        [Required(ErrorMessage = "O campo é obrigatório.")]
        public int Id { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "O Id do proprietário deve ser maior que zero.")]
        public int IdProprietario { get; set; }

        [Display(Name = "Está alugado?")]
        [Required]
        [Range(0, 1, ErrorMessage = "O valor deve ser 0 (não alugado) ou 1 (alugado).")]
        public sbyte EstaAlugado { get; set; }

        [Required(ErrorMessage = "O campo é obrigatório.")]
        [StringLength(50, ErrorMessage = "O campo deve ter no máximo 50 caracteres.")]
        public string Apelido { get; set; } = null!;

        [Required(ErrorMessage = "O campo é obrigatório.")]
        [StringLength(100, ErrorMessage = "O campo deve ter no máximo 100 caracteres.")]
        public string Logradouro { get; set; } = null!;

        [Required(ErrorMessage = "O campo é obrigatório.")]
        [StringLength(2, ErrorMessage = "O campo deve ter exatamente 2 caracteres.", MinimumLength = 2)]
        public string Uf { get; set; } = null!;

        [Required(ErrorMessage = "O campo é obrigatório.")]
        [StringLength(8, ErrorMessage = "O campo deve ter exatamente 8 caracteres.", MinimumLength = 8)]
        public string Cep { get; set; } = null!;

        [Required(ErrorMessage = "O campo é obrigatório.")]
        [StringLength(10, ErrorMessage = "O campo deve ter no máximo 10 caracteres.")]
        public string Numero { get; set; } = null!;

        [Required(ErrorMessage = "O campo é obrigatório.")]
        [StringLength(50, ErrorMessage = "O campo deve ter no máximo 50 caracteres.")]
        public string Cidade { get; set; } = null!;

        [Required(ErrorMessage = "O campo é obrigatório.")]
        [StringLength(50, ErrorMessage = "O campo deve ter no máximo 50 caracteres.")]
        public string Bairro { get; set; } = null!;
    }
}
