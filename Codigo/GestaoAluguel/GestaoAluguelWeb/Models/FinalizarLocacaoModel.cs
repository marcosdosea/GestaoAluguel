using System;
using Microsoft.AspNetCore.Http;

namespace GestaoAluguelWeb.Models
{
    public class FinalizarLocacaoModel
    {
        public int LocacaoId { get; set; }
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