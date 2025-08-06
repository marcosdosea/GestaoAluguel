using System.ComponentModel.DataAnnotations;

namespace Core.DTO
{
    public class ChamadoReparoDTO
    {
        public int Id { get; set; }

        /// <summary>
        /// C - Cadastrada
        /// P - Em progresso
        /// R - Resolvido
        /// </summary>
        public string Status { get; set; } = null!;

        public DateTime DataCadastro { get; set; }

        public sbyte EstaResolvido { get; set; }

        [Display(Name = "Código do imóvel")]
        public int IdImovel { get; set; }
    }
}
