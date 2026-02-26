using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace GestaoAluguelWeb.Models
{
    public class ChamadoReparoModel
    {
        [Key]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Status")]
        [StringLength(1, ErrorMessage = "O status deve ter 1 caractere: C, P ou R.")]
        public string Status { get; set; } = "C";

        [Display(Name = "Tipo")]
        public string? Tipo { get; set; }

        [Display(Name = "Descrição")]
        [StringLength(500)]
        public string? Descricao { get; set; }

        [Display(Name = "Data de Cadastro")]
        [DataType(DataType.DateTime)]
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        [Display(Name = "Data de Resolução")]
        [DataType(DataType.DateTime)]
        public DateTime? DataResolucao { get; set; }

        [Display(Name = "Está Resolvido")]
        public bool EstaResolvido
        {
            get => Status == "R";
            set => Status = value ? "R" : Status;
        }

        [Required]
        [Display(Name = "Imóvel")]
        public int IdImovel { get; set; }

        [ValidateNever]
        public String? ApelidoImovel { get; set; }

        [ValidateNever]
        public String? EnderecoImovel { get; set; }

        [Required]
        [Display(Name = "Inquilino")]
        public int IdInquilino { get; set; }

        public bool EmProgresso() => Status == "P";
        public bool Cadastrada() => Status == "C";
        public bool Resolvida() => Status == "R";
    }
}
