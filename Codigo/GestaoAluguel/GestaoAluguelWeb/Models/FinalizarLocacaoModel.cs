using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace GestaoAluguelWeb.Models
{
    public class FinalizarLocacaoModel
    {
        [Display(Name = "Código")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public int Id { get; set; }

        public LocacaoModel? Locacao { get; set; }
        public string? ImovelInfo { get; set; }
        public string? InquilinoInfo { get; set; }
        public string? StatusLocacao { get; set; }
        public int DuracaoLocacao { get; set; }
        public decimal ValorTotalEstimado { get; set; }

        public DateTime DataFim { get; set; }
        public string? Motivo { get; set; }
        public IFormFile? ContratoEncerramento { get; set; }
    }
}