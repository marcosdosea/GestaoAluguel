using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class PagamentoDTO
    {
        public int Id { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataPagamento { get; set; }
        public enum TipoPagamento
        {
            Pix,
            Debito,
            Credito,
            Especime
        }
    }
}
