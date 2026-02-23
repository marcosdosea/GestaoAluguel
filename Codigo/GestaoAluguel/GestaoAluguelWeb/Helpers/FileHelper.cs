using Microsoft.AspNetCore.Http;
using System.Text;

namespace GestaoAluguelWeb.Helpers
{
    public class FileHelper
    {

        /// <summary>
        /// Enum para representar os tipos de arquivo suportados.
        /// </summary>
        public enum FileType
        {
            Unknown,
            Pdf,
            Jpeg,
            Png,
            Gif,
            Bmp,
            WebP,
            Docx
        }

        // Dicionário com as assinaturas de bytes para cada tipo de arquivo.
        private static readonly Dictionary<FileType, List<byte[]>> _signatures = new()
    {
        { FileType.Pdf, new List<byte[]> { new byte[] { 0x25, 0x50, 0x44, 0x46 } } }, // %PDF
        { FileType.Jpeg, new List<byte[]>
            {
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
                new byte[] { 0xFF, 0xD8, 0xFF, 0xDB }
            }
        },
        { FileType.Png, new List<byte[]> { new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A } } },
        { FileType.Gif, new List<byte[]> { new byte[] { 0x47, 0x49, 0x46, 0x38 } } },
        { FileType.Bmp, new List<byte[]> { new byte[] { 0x42, 0x4D } } },
        { FileType.WebP, new List<byte[]> { new byte[] { 0x52, 0x49, 0x46, 0x46 } } },
        // Arquivos .docx, .xlsx, .pptx são baseados em ZIP, que começa com "PK"
        { FileType.Docx, new List<byte[]> { new byte[] { 0x50, 0x4B, 0x03, 0x04 } } }
    };

        // Mapeamento de FileType para seu MIME type correspondente, essencial para o Data URL.
        private static readonly Dictionary<FileType, string> _mimeTypes = new()
    {
        { FileType.Pdf, "application/pdf" },
        { FileType.Jpeg, "image/jpeg" },
        { FileType.Png, "image/png" },
        { FileType.Gif, "image/gif" },
        { FileType.Bmp, "image/bmp" },
        { FileType.WebP, "image/webp" },
        { FileType.Docx, "application/vnd.openxmlformats-officedocument.wordprocessingml.document" }
    };

        /// <summary>
        /// Identifica o tipo de arquivo com base em sua assinatura de bytes.
        /// </summary>
        public static FileType GetFileType(IFormFile file)
        {
            if (file == null || file.Length == 0) return FileType.Unknown;
            using var stream = file.OpenReadStream();
            return GetFileType(stream);
        }

        public static FileType GetFileType(Stream stream)
        {
            if (stream == null || stream.Length == 0 || !stream.CanRead) return FileType.Unknown;
            long originalPosition = stream.Position;
            try {
                stream.Position = 0;
                var maxSignatureLength = _signatures.Values.SelectMany(s => s).Max(m => m.Length);
                var headerBytes = new byte[maxSignatureLength];
                int bytesRead = stream.Read(headerBytes, 0, maxSignatureLength);

                if (bytesRead < 4) return FileType.Unknown; // Arquivo muito pequeno

                foreach (var (type, signatures) in _signatures)
                {
                    foreach (var signature in signatures)
                    {
                        if (headerBytes.Take(signature.Length).SequenceEqual(signature))
                        {
                            // Caso especial: WebP começa com RIFF, mas precisa ter "WEBP" nos bytes 8-11
                            if (type == FileType.WebP)
                            {
                                if (bytesRead >= 12 &&
                                    headerBytes[8] == 0x57 && headerBytes[9] == 0x45 &&
                                    headerBytes[10] == 0x42 && headerBytes[11] == 0x50) // 'W' 'E' 'B' 'P'
                                {
                                    stream.Position = originalPosition;
                                    return FileType.WebP;
                                }
                                continue; // Se for RIFF mas não for WEBP (pode ser AVI/WAV), continua procurando
                            }
                            stream.Position = originalPosition;
                            return type;
                        }
                    }
                }
                return FileType.Unknown;
            }finally
            {
                // Sempre devolve o ponteiro para o começo!
                stream.Position = originalPosition;
            }
        }

        /// <summary>
        /// NOVO: Verifica se o tipo real de um arquivo está dentro de uma lista de tipos permitidos.
        /// </summary>
        /// <param name="file">O IFormFile a ser validado.</param>
        /// <param name="allowedTypes">Uma lista de FileTypes que são permitidos.</param>
        /// <returns>Verdadeiro se o tipo do arquivo for permitido, falso caso contrário.</returns>
        public static bool IsValid(IFormFile file, params FileType[] allowedTypes)
        {
            if (allowedTypes == null || allowedTypes.Length == 0)
            {
                return false; // Se nada é permitido, nada é válido.
            }

            var actualType = GetFileType(file);
            return allowedTypes.Contains(actualType);
        }

        public static bool IsValid(Byte[] file, params FileType[] allowedTypes)
        {
            if (allowedTypes == null || allowedTypes.Length == 0)
            {
                return false; // Se nada é permitido, nada é válido.
            }

            var actualType = GetFileType(file);
            return allowedTypes.Contains(actualType);
        }

        /// <summary>
        /// NOVO: Identifica o tipo de arquivo a partir de um array de bytes.
        /// </summary>
        /// <param name="fileBytes">O conteúdo do arquivo como byte[].</param>
        /// <returns>O FileType correspondente.</returns>
        public static FileType GetFileType(byte[] fileBytes)
        {
            if (fileBytes == null || fileBytes.Length == 0)
            {
                return FileType.Unknown;
            }
            // Reutiliza a lógica principal convertendo o byte[] para um MemoryStream
            using (var stream = new MemoryStream(fileBytes))
            {
                return GetFileType(stream);
            }
        }

        /// <summary>
        /// NOVO: Gera um Data URL (string Base64) a partir de um array de bytes.
        /// </summary>
        /// <param name="fileBytes">O conteúdo do arquivo como byte[].</param>
        /// <returns>Uma string Data URL ou null se o tipo de arquivo for desconhecido.</returns>
        public static string? GetDataUrl (byte[] fileBytes)
        {
            if (fileBytes == null || fileBytes.Length == 0) return null;

            var fileType = GetFileType(fileBytes);

            // Se não achou o tipo exato, mas sabemos que é bytes, tenta forçar Jpeg ou Png como fallback
            // ou retorna null se for desconhecido strict.
            if (fileType == FileType.Unknown)
            {
                // Tenta detectar se é JPEG ou PNG mesmo sem assinatura clara, baseado em heurística simples
                if (fileBytes.Length > 4)
                {
                    if (fileBytes[0] == 0xFF && fileBytes[1] == 0xD8) // JPEG
                    {
                        fileType = FileType.Jpeg;
                    }
                    else if (fileBytes[0] == 0x89 && fileBytes[1] == 0x50 && fileBytes[2] == 0x4E && fileBytes[3] == 0x47) // PNG
                    {
                        fileType = FileType.Png;
                    }
                    else
                    {
                        return null; // Tipo desconhecido, não conseguimos identificar
                    }
                }
            }

            if (!_mimeTypes.TryGetValue(fileType, out var mimeType))
            {
                // Fallback amigável: se não descobriu, assume jpeg pra tentar mostrar
                mimeType = "image/jpeg";
            }

            var base64String = Convert.ToBase64String(fileBytes);
            return $"data:{mimeType};base64,{base64String}";
        }

        /// <summary>
        /// NOVO: Gera um Data URL (string Base64) para um arquivo, pronto para ser usado no atributo 'src' de tags HTML.
        /// </summary>
        /// <param name="file">O arquivo a ser convertido.</param>
        /// <returns>Uma string Data URL ou null se o tipo de arquivo for desconhecido.</returns>
        public static async Task<string?> GetDataUrlAsync(IFormFile file)
        {
            var arquivoBytes = await ConverterParaBytes(file);
            return GetDataUrl(arquivoBytes ?? Array.Empty<byte>());
        }

        /// <summary>
        /// Converte o IFormFile para Byte[] (útil para salvar no banco).
        /// </summary>

        public static async Task<byte[]?> ConverterParaBytes(IFormFile file)
        {
            if (file == null) return null;

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

    }

}

