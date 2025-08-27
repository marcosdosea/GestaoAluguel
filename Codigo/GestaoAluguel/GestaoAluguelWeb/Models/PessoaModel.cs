using Core;
using System.ComponentModel.DataAnnotations;
using Util;

namespace GestaoAluguelWeb.Models
{
    public class PessoaModel
    {
        [Display(Name = "Código")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public int Id { get; set; }

        [StringLength(45,MinimumLength = 5, ErrorMessage = "O nome deve ter obrigatóriamente entre 5 e 45 caracteres")]
        public string Nome { get; set; } = null!;

        [CPF]
        public string Cpf { get; set; } = null!;

        [RG]
        public string? Rg { get; set; }

        [EmailAddress(ErrorMessage = "O e-mail informado não é válido.")]
        public string Email { get; set; } = null!;

        [TelefoneCelular]
        public string Telefone { get; set; } = null!;

        /// <summary>
        /// S - Solteiro
        /// C - Casado
        /// D - Divorciado
        /// U - União Estável
        /// V - Viúvo
        /// 
        /// </summary>
        [Display(Name = "Estado Civil")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string EstadoCivil { get; set; } = null!;

        [Display(Name = "Profissão")]
        [StringLength(45, MinimumLength = 1, ErrorMessage = "A profissão é obrigatório e deve ter no máximo 45 caracteres")]
        public string Profissao { get; set; } = null!;

        [StringLength(45, ErrorMessage = "O {0} é obrigatório e deve ter no máximo 45 caracteres")]
        public string Logradouro { get; set; } = null!;

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Uf { get; set; } = null!;

        [StringLength(6,MinimumLength = 1, ErrorMessage = "O número da casa é obrigatório deve ter no máximo 6 caracteres")]
        public string Numero { get; set; } = null!;

        [StringLength(45, MinimumLength = 1, ErrorMessage = "A cidade é obrigatória e deve ter no máximo 45 caracteres")]
        public string Cidade { get; set; } = null!;

        [StringLength(45, MinimumLength = 1, ErrorMessage = "O bairro é obrigatório e deve ter no máximo 45 caracteres")]
        public string Bairro { get; set; } = null!;

        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date , ErrorMessage = "Data inválida")]
        [Required(ErrorMessage = "A {0} é obrigatória.")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",ApplyFormatInEditMode = true)]
        public DateTime Nascimento { get; set; }

        [Display(Name = "CEP")]
        [Cep]
        public string Cep { get; set; } = null!;

        [Display(Name = "Nome do Cônjuge")]
        [StringLength(45, ErrorMessage = "O nome do cônjuge deve ter no máximo 45 caracteres")]
        public string? NomeConjuge { get; set; }

        [Display(Name = "Foto de perfil")]
        public byte[]? Foto { get; set; }

        public virtual ICollection<Chamadoreparo> Chamadoreparos { get; set; } = [];

        public virtual ICollection<Imovel> Imovels { get; set; } = [];

        public virtual ICollection<Locacao> Locacaos { get; set; } = [];

    }
}
