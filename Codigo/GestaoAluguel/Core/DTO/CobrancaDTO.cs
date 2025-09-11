using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class CobrancaDTO
    {
        public int Id { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataCobranca { get; set; }
        public enum TipoCobranca
        {
            Aluguel,
            Agua,
            Luz,
            Gas,
            Internet,
            Condomino,
            Reparo,
            Outros
        }
        public string Descricao { get; set; }
        public int IdLocacao { get; set; }
    }
}
