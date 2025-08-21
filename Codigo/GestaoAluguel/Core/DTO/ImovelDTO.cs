using System.ComponentModel.DataAnnotations;

namespace Core.DTO
{
    public class ImovelDTO
    {
        public int Id { get; set; }

        public sbyte EstaAlugado { get; set; }

        public string Apelido { get; set; } = null!;

        public string Logradouro { get; set; } = null!;

        public string numero { get; set; } = null!;

        public int idProprietario { get; set; }

        [Display(Name = "Código do proprietário")]
        public int IdProprietario { get; set; }
    }
}
