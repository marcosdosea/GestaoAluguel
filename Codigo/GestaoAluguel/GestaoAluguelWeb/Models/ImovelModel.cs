using System.ComponentModel.DataAnnotations;

namespace GestaoAluguelWeb.Models
{
    public class ImovelModel
    {
        [Display(Name = "Código")]
        [Key]
        [Required(ErrorMessage = "O campo é obrigatório.")]
        public int Id { get; set; }

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
        [StringLength(9, ErrorMessage = "O campo deve ter exatamente 9 caracteres.", MinimumLength = 9)]
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
