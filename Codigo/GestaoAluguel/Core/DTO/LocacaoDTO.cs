using System.ComponentModel.DataAnnotations;

namespace Core.DTO
{
    public class LocacaoDTO
    {
        public int Id { get; set; }

        public DateTime DataInicio { get; set; }

        public DateTime? DataFim { get; set; }

        /// <summary>
        /// 0 - Ativo
        /// 1 - Cancelado
        /// 2 - Finalizado
        /// </summary>
        public sbyte Status { get; set; }

        [Display(Name = "Código do imóvel")]
        public int IdImovel { get; set; }

        [Display(Name = "Código do inquilino")]
        public int IdInquilino { get; set; }
    }
}
