using GestaoAluguelWeb.Helpers;

namespace GestaoAluguelWeb.Models
{
    public class ArquivoModel
    {

        public string? DataUrl { get; set; }
        public FileHelper.FileType TipoArquivo { get; set; }

    }
}
