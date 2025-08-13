using System.ComponentModel.DataAnnotations;

namespace Core.DTO
{
    public class ImovelDTO
    {
        public int Id { get; set; }

        public sbyte EstaAlugado { get; set; }

        public string Apelido { get; set; } = null!;

        [Display(Name = "Código do proprietário")]
        public int IdProprietario { get; set; }
    }
}
